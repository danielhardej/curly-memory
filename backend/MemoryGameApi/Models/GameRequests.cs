namespace MemoryGameApi.Models
{
    public class CreateGameRequest
    {
        public int GridSize { get; set; } = 4; // 4x4 grid by default
    }

    public class FlipCardRequest
    {
        public int CardId { get; set; }
    }

    public class GameResponse
    {
        public string Id { get; set; } = string.Empty;
        public List<Card> Cards { get; set; } = new List<Card>();
        public GameStatus Status { get; set; }
        public int Moves { get; set; }
        public int MatchedPairs { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? DurationFormatted { get; set; }
        public bool IsGameComplete { get; set; }
    }
}