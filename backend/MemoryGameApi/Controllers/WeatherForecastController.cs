using Microsoft.AspNetCore.Mvc;
using MemoryGameApi.Models;
using MemoryGameApi.Services;
using System.Text.RegularExpressions;

namespace MemoryGameApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService gameService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<GameResponse>> CreateGame([FromBody] CreateGameRequest request)
    {
        try
        {
            var game = await _gameService.CreateGameAsync(request.GridSize);
            var response = MapToResponse(game);
            _logger.LogInformation("Created new game with ID: {GameId}", game.Id);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid game creation request: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{gameId}")]
    public async Task<ActionResult<GameResponse>> GetGame(string gameId)
    {
        var game = await _gameService.GetGameAsync(gameId);
        if (game == null)
        {
            _logger.LogWarning("Game not found: {GameId}", SanitizeForLogging(gameId));
            return NotFound($"Game with ID {gameId} not found");
        }

        var response = MapToResponse(game);
        return Ok(response);
    }

    [HttpPost("{gameId}/flip")]
    public async Task<ActionResult<GameResponse>> FlipCard(string gameId, [FromBody] FlipCardRequest request)
    {
        var game = await _gameService.FlipCardAsync(gameId, request.CardId);
        if (game == null)
        {
            _logger.LogWarning("Game not found for flip action: {GameId}", SanitizeForLogging(gameId));
            return NotFound($"Game with ID {gameId} not found");
        }

        var response = MapToResponse(game);
        _logger.LogInformation("Card {CardId} flipped in game {GameId}", request.CardId, SanitizeForLogging(gameId));
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<GameResponse>>> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        var responses = games.Select(MapToResponse).ToList();
        return Ok(responses);
    }

    private static GameResponse MapToResponse(Game game)
    {
        return new GameResponse
        {
            Id = game.Id,
            Cards = game.Cards,
            Status = game.Status,
            Moves = game.Moves,
            MatchedPairs = game.MatchedPairs,
            CreatedAt = game.CreatedAt,
            CompletedAt = game.CompletedAt,
            Duration = game.Duration,
            DurationFormatted = game.DurationFormatted,
            IsGameComplete = game.Status == GameStatus.Completed
        };
    }

    private static string SanitizeForLogging(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        // Remove any line breaks and limit length to prevent log forging
        return Regex.Replace(input, @"[\r\n\t]", " ").Substring(0, Math.Min(input.Length, 100));
    }
}
