import React, { useState, useEffect, useCallback } from 'react';
import { Game, GameStatus } from '../types/game';
import gameService from '../services/gameService';
import Card from './Card';
import './GameBoard.css';

const GameBoard: React.FC = () => {
  const [game, setGame] = useState<Game | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [gridSize, setGridSize] = useState(4);
  const [flippedCards, setFlippedCards] = useState<number[]>([]);

  const createNewGame = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const newGame = await gameService.createGame({ gridSize });
      setGame(newGame);
      setFlippedCards([]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create game');
    } finally {
      setLoading(false);
    }
  }, [gridSize]);

  const handleCardClick = async (cardId: number) => {
    if (!game || game.status !== GameStatus.InProgress || flippedCards.length >= 2) {
      return;
    }

    if (flippedCards.includes(cardId)) {
      return;
    }

    const newFlippedCards = [...flippedCards, cardId];
    setFlippedCards(newFlippedCards);

    try {
      const updatedGame = await gameService.flipCard(game.id, { cardId });
      setGame(updatedGame);

      // Reset flipped cards after a short delay if two cards are flipped
      if (newFlippedCards.length === 2) {
        setTimeout(() => {
          setFlippedCards([]);
        }, 1000);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to flip card');
      setFlippedCards(flippedCards);
    }
  };

  const getGridClass = () => {
    return `game-grid grid-${gridSize}x${gridSize}`;
  };



  useEffect(() => {
    createNewGame();
  }, [createNewGame]);

  if (loading) {
    return <div className="loading">Creating new game...</div>;
  }

  return (
    <div className="game-board">
      <h1>Memory Game</h1>
      
      <div className="game-controls">
        <div className="grid-size-selector">
          <label htmlFor="gridSize">Grid Size: </label>
          <select
            id="gridSize"
            value={gridSize}
            onChange={(e) => setGridSize(parseInt(e.target.value))}
            disabled={game?.status === GameStatus.InProgress}
          >
            <option value={2}>2x2</option>
            <option value={4}>4x4</option>
            <option value={6}>6x6</option>
          </select>
        </div>
        
        <button onClick={createNewGame} className="new-game-btn">
          New Game
        </button>
      </div>

      {error && <div className="error">Error: {error}</div>}

      {game && (
        <div className="game-info">
          <div className="game-stats">
            <span>Moves: {game.moves}</span>
            <span>Matched Pairs: {game.matchedPairs}/{game.cards.length / 2}</span>
            {game.status === GameStatus.Completed && game.durationFormatted && (
              <span>Time: {game.durationFormatted}</span>
            )}
          </div>
          
          {game.status === GameStatus.Completed && (
            <div className="victory-message">
              🎉 Congratulations! You completed the game in {game.moves} moves!
            </div>
          )}
        </div>
      )}

      {game && (
        <div className={getGridClass()}>
          {game.cards.map((card) => (
            <Card
              key={card.id}
              card={card}
              onCardClick={handleCardClick}
              disabled={flippedCards.length >= 2 && !flippedCards.includes(card.id)}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default GameBoard;