namespace BrickOutApi.Interfaces
{
    public interface IScoreService
    {
        Task<int> GetScoreAsync(int userId);
        Task<bool> AddScoreAsync(int userId, int scoreToAdd);
    }
}
