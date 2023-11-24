﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Entities;
using ProjectChronos.Extensions;
using ProjectChronos.Models.Requests;

namespace ProjectChronos.Controllers
{
    public class GameSystemController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ICardDeckService _cardDeckService;
        private readonly IGameSystemService _gameSystemService;
        private readonly IMemoryCache _memoryCache;


        public GameSystemController(SignInManager<User> signInManager, UserManager<User> userManager, ICardDeckService cardDeckService, IGameSystemService gameSystemService, IMemoryCache memoryCache)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cardDeckService = cardDeckService;
            _gameSystemService = gameSystemService;
            _memoryCache = memoryCache;
        }

        // Get opponents
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOpponents()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var opponents = await _gameSystemService.GetOrCreateUserOpponentsAsync(currentUser);
            if (opponents == null)
            {
                return BadRequest();
            }
            return Ok(opponents.ToDto());
        }

        // Initiate match
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InitiateMatch([FromBody] int opponentId)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var matchInstance = await _gameSystemService.InitiateMatchAsync(currentUser, opponentId);
            if (matchInstance == null)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
