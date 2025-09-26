using MemoryGameApi.Models;
using MemoryGameApi.Services;

namespace MemoryGameApi.Tests;

public class GameServiceTests
{
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameService = new GameService();
    }

    [Fact]
    public async Task CreateGameAsync_ValidGridSize_CreatesGameWithCorrectNumberOfCards()
    {
        // Arrange
        int gridSize = 4;
        int expectedCardCount = gridSize * gridSize;

        // Act
        var game = await _gameService.CreateGameAsync(gridSize);

        // Assert
        Assert.NotNull(game);
        Assert.Equal(expectedCardCount, game.Cards.Count);
        Assert.Equal(GameStatus.InProgress, game.Status);
        Assert.Equal(0, game.Moves);
        Assert.Equal(0, game.MatchedPairs);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(9)]
    public async Task CreateGameAsync_InvalidGridSize_ThrowsArgumentException(int gridSize)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _gameService.CreateGameAsync(gridSize));
    }

    [Fact]
    public async Task CreateGameAsync_CreatesCardPairs()
    {
        // Arrange
        int gridSize = 4;

        // Act
        var game = await _gameService.CreateGameAsync(gridSize);

        // Assert
        var cardGroups = game.Cards.GroupBy(c => c.Value);
        Assert.All(cardGroups, group => Assert.Equal(2, group.Count()));
    }

    [Fact]
    public async Task FlipCardAsync_ValidCard_FlipsCard()
    {
        // Arrange
        var game = await _gameService.CreateGameAsync(4);
        var cardToFlip = game.Cards.First();

        // Act
        var result = await _gameService.FlipCardAsync(game.Id, cardToFlip.Id);

        // Assert
        Assert.NotNull(result);
        Assert.True(cardToFlip.IsFlipped);
    }

    [Fact]
    public async Task FlipCardAsync_TwoMatchingCards_MarksAsMatched()
    {
        // Arrange
        var game = await _gameService.CreateGameAsync(4);
        var matchingCards = game.Cards.GroupBy(c => c.Value).First(g => g.Count() == 2).ToList();

        // Act
        await _gameService.FlipCardAsync(game.Id, matchingCards[0].Id);
        var result = await _gameService.FlipCardAsync(game.Id, matchingCards[1].Id);

        // Assert
        Assert.NotNull(result);
        Assert.True(matchingCards[0].IsMatched);
        Assert.True(matchingCards[1].IsMatched);
        Assert.Equal(1, result.MatchedPairs);
        Assert.Equal(1, result.Moves);
    }

    [Fact]
    public async Task GetGameAsync_ExistingGame_ReturnsGame()
    {
        // Arrange
        var createdGame = await _gameService.CreateGameAsync(4);

        // Act
        var result = await _gameService.GetGameAsync(createdGame.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdGame.Id, result.Id);
    }

    [Fact]
    public async Task GetGameAsync_NonExistentGame_ReturnsNull()
    {
        // Act
        var result = await _gameService.GetGameAsync("non-existent-id");

        // Assert
        Assert.Null(result);
    }
}
