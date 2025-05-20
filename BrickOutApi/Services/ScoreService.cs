using BrickOutApi.DataBase;
using BrickOutApi.Interfaces;
using BrickOutApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrickOutApi.Services
{
    public class ScoreService : IScoreService
    {
        private readonly AppDbContext _context;

        public ScoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetScoreAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Score ?? 0; // Возвращаем 0, если пользователь не найден
        }

        public async Task<bool> AddScoreAsync(int userId, int scoreToAdd)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; // Пользователь не найден
            }

            user.Score += scoreToAdd;
            await _context.SaveChangesAsync();
            return true; // Успешно добавлено
        }
    }
}
