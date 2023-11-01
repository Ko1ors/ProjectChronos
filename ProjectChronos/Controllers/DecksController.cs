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

        // Get active deck
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetActiveDeck()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(_cardDeckService.GetActiveUserDeck(currentUser));
        }

        // Get all card decks
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCardDecks()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(_cardDeckService.GetAllUserDecks(currentUser));
        }


        // Create new card deck
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCardDeck([FromBody] CreateCardDeckRequest request)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(await _cardDeckService.CreateCardDeckAsync(currentUser, request.Cards.Select(c => c.ToDeckCard()), request.Active));
        }

        // Update existing card deck
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCardDeck([FromBody] UpdateCardDeckRequest request)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(await _cardDeckService.UpdateCardDeckAsync(currentUser, request.DeckId, request.Cards.Select(c => c.ToDeckCard()), request.Active));
        }

        // Delete existing card deck
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCardDeck(int deckId)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(_cardDeckService.DeleteCardDeck(currentUser, deckId));
        }

        // Mark card deck as active
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> MarkAsActive(int deckId)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            return Ok(_cardDeckService.MarkAsActive(currentUser, deckId));
        }
    }
}
