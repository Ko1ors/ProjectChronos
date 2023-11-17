using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.Common.Models.ExpressApi;
using ProjectChronos.Entities;
using ProjectChronos.Models.Requests;

namespace ProjectChronos.Controllers
{
    public class PacksController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ICardPackService _cardPackService;
        private readonly IMemoryCache _memoryCache;

        public PacksController(SignInManager<User> signInManager, UserManager<User> userManager, ICardPackService cardPackService, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cardPackService = cardPackService;
            _memoryCache = memoryCache;
        }

        // Claim Pack
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseServiceResult>> ClaimPack([FromBody] ClaimPackRequest model)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(await _cardPackService.ClaimPackAsync(currentUser, model.PackType));
        }

        // Get Pack Content
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ExpressPackContent>> GetPackContent(CardPackType type)
        {
            return Ok(await _cardPackService.GetPackContentAsync(type));
        }

        // Get Owned Packs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ExpressPack>>> GetOwnedPacks()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(await _cardPackService.GetOwnedPacksAsync(currentUser));
        }
    }
}
