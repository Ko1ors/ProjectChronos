using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Entities;
using ProjectChronos.Models.Requests;

namespace ProjectChronos.Controllers
{
    public class DecksController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ICardDeckService _cardDeckService;
        private readonly IMemoryCache _memoryCache;


        public DecksController(SignInManager<User> signInManager, UserManager<User> userManager, ICardDeckService cardDeckService, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cardDeckService = cardDeckService;
            _memoryCache = memoryCache;
        }


        // Create new card deck
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCardDeck([FromBody] CreateCardDeckRequest request)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(_cardDeckService.CreateCardDeck(currentUser, request.Cards.Select(c => c.ToDeckCard())));
        }
    }
}
