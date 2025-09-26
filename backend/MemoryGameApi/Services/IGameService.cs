using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(int gridSize);
        Task<Game?> GetGameAsync(string gameId);
        Task<Game?> FlipCardAsync(string gameId, int cardId);
        Task<List<Game>> GetAllGamesAsync();
    }
}