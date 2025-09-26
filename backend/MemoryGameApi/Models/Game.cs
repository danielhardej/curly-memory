namespace MemoryGameApi.Models
{
    public enum GameStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class Game
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<Card> Cards { get; set; } = new List<Card>();
        public GameStatus Status { get; set; } = GameStatus.NotStarted;
        public int Moves { get; set; }
        public int MatchedPairs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? Duration => CompletedAt?.Subtract(CreatedAt);
    }
}