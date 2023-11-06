using ProjectChronos.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.RegularExpressions;

namespace ProjectChronos.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IPolygonService _polygonService;
        private readonly IMemoryCache _memoryCache;

        public UsersController(SignInManager<User> signInManager, UserManager<User> userManager, IPolygonService polygonService, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _polygonService = polygonService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult UserAuthorized()
        {
            return Ok(!string.IsNullOrWhiteSpace(User.Identity?.Name));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UserAdmin()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(await _userManager.IsInRoleAsync(currentUser, "Administrator"));
        }

        [HttpGet]
        public IActionResult GetAuthMessage(string address)
        {
            // Validate address
            if (string.IsNullOrEmpty(address) || !new Regex(@"^0x[a-fA-F0-9]{40}$").IsMatch(address))
            {
                return BadRequest("Invalid address");
            }
            string key = $"auth_{address}";

            if (!_memoryCache.TryGetValue(key, out string? message))
            {
                message = _polygonService.GenerateAuthMessage(address);
            }

            // cache message with a predefined cache key and SlidingExpiration
            // this allows the item to be removed automatically if it has not been accessed in the last SlidingExpiration time span
            _memoryCache.Set(key, message, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));

            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login attempt");
            }

            string key = $"auth_{model.Address}";

            if (!_memoryCache.TryGetValue(key, out string? message))
            {
                return BadRequest("Invalid login attempt");
            }

            // Validate signature
            // this should also validate that the address is a valid address
            var isSignatureValid = await _polygonService.ValidateSignedMessageAsync(model.Address, message, model.Signature);
            if (!isSignatureValid)
            {
                return BadRequest("Invalid signature");
            }

            // Check if user exists
            var user = await _signInManager.UserManager.FindByNameAsync(model.Address);

            // If user does not exist, create the user
            if (user == null)
            {
                user = await CreateUserAsync(model.Address);
            }

            // Clear cache
            _memoryCache.Remove(key);

            await _signInManager.SignInAsync(user, model.RememberMe);
            return Ok("User login successfully");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid register attempt");
            }

            if (!string.Equals(model.Password, model.ConfirmPassword))
            {
                return BadRequest("Passwords do not match");
            }

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _signInManager.UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok("User registered successfully");
            }
            if (result.Errors.Any())
            {
                return BadRequest(result.Errors.First().Description);
            }
            return BadRequest("Invalid register attempt");
        }


        private async Task<User> CreateUserAsync(string address)
        {
            var user = new User { UserName = address };
            var result = await _signInManager.UserManager.CreateAsync(user);
            if (result.Succeeded)
                return await _userManager.FindByNameAsync(address);
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AssignAdminRole([FromBody] string userId)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest("User not found");
            }

            await _signInManager.UserManager.AddToRoleAsync(user, "Administrator");
            return Ok("Administrator role was assigned successfully");
        }
    }
}
