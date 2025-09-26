namespace MemoryGameApi.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
}