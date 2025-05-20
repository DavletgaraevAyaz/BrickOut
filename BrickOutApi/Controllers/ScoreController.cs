using BrickOutApi.Interfaces;
using BrickOutApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrickOutApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Защита контроллера, чтобы доступ был только для авторизованных пользователей
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetScore(int userId)
        {
            var score = await _scoreService.GetScoreAsync(userId);
            return Ok(new { Score = score });
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddScore(int userId, [FromBody] AddCoinsRequest scoreToAdd)
        {
            if (scoreToAdd.amount <= 0)
            {
                return BadRequest("Количество очков должно быть положительным");
            }

            var result = await _scoreService.AddScoreAsync(userId, scoreToAdd.amount);
            if (!result)
            {
                return NotFound("Пользователь не найден");
            }

            var score = await _scoreService.GetScoreAsync(userId);
            return Ok(new { Score = score });
        }
    }
}

public class AddCoinsRequest
{
    public int amount { get; set; }
}