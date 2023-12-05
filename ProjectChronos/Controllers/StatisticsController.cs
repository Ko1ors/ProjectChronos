using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Entities;

namespace ProjectChronos.Controllers
{
    public class StatisticsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStatsService _userStatsService;

        public StatisticsController(UserManager<User> userManager, IUserStatsService userStatsService)
        {
            _userManager = userManager;
            _userStatsService = userStatsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserStatistics()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name);
            var stats = await _userStatsService.GetCompositeUserStatsAsync(currentUser);
            return Ok(stats);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserStatisticsByAddress(string address)
        {
            var user = await _userManager.FindByNameAsync(address);
            var stats = await _userStatsService.GetCompositeUserStatsAsync(user);
            return Ok(stats);
        }
    }
}
