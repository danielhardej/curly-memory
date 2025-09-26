using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public class GameService : IGameService
    {
        private static readonly Dictionary<string, Game> _games = new();
        private static readonly string[] _cardValues = 
        {
            "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼",
            "🐨", "🐯", "🦁", "🐸", "🐵", "🐔", "🐧", "🐦",
            "🦋", "🐛", "🐜", "🐝", "🐞", "🦗", "🕷", "🦂"
        };

        public Task<Game> CreateGameAsync(int gridSize)
        {
            if (gridSize < 2 || gridSize > 8 || gridSize % 2 != 0)
            {
                throw new ArgumentException("Grid size must be an even number between 2 and 8");
            }

            var game = new Game();
            var totalCards = gridSize * gridSize;
            var pairsNeeded = totalCards / 2;

            if (pairsNeeded > _cardValues.Length)
            {
                throw new ArgumentException($"Not enough card values for {gridSize}x{gridSize} grid");
            }

            // Create pairs of cards
            var cards = new List<Card>();
            for (int i = 0; i < pairsNeeded; i++)
            {
                var value = _cardValues[i];
                cards.Add(new Card { Id = i * 2, Value = value });
                cards.Add(new Card { Id = i * 2 + 1, Value = value });
            }

            // Shuffle the cards
            var random = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }

            game.Cards = cards;
            game.Status = GameStatus.InProgress;

            _games[game.Id] = game;
            return Task.FromResult(game);
        }

        public Task<Game?> GetGameAsync(string gameId)
        {
            _games.TryGetValue(gameId, out var game);
            return Task.FromResult(game);
        }

        public Task<Game?> FlipCardAsync(string gameId, int cardId)
        {
            if (!_games.TryGetValue(gameId, out var game))
            {
                return Task.FromResult<Game?>(null);
            }

            if (game.Status != GameStatus.InProgress)
            {
                return Task.FromResult<Game?>(game);
            }

            var card = game.Cards.FirstOrDefault(c => c.Id == cardId);
            if (card == null || card.IsMatched || card.IsFlipped)
            {
                return Task.FromResult<Game?>(game);
            }

            // Get currently flipped cards
            var flippedCards = game.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();

            if (flippedCards.Count >= 2)
            {
                // Reset previously flipped cards that didn't match
                foreach (var flippedCard in flippedCards)
                {
                    flippedCard.IsFlipped = false;
                }
            }

            // Flip the selected card
            card.IsFlipped = true;
            
            // Check for matches after flipping
            var currentFlippedCards = game.Cards.Where(c => c.IsFlipped && !c.IsMatched).ToList();
            
            if (currentFlippedCards.Count == 2)
            {
                game.Moves++;
                
                if (currentFlippedCards[0].Value == currentFlippedCards[1].Value)
                {
                    // Match found
                    foreach (var matchedCard in currentFlippedCards)
                    {
                        matchedCard.IsMatched = true;
                        matchedCard.IsFlipped = false; // Cards stay visible but not flipped
                    }
                    game.MatchedPairs++;
                }
            }

            // Check if game is complete
            if (game.MatchedPairs == game.Cards.Count / 2)
            {
                game.Status = GameStatus.Completed;
                game.CompletedAt = DateTime.UtcNow;
            }

            return Task.FromResult<Game?>(game);
        }

        public Task<List<Game>> GetAllGamesAsync()
        {
            return Task.FromResult(_games.Values.ToList());
        }
    }
}