export interface Card {
  id: number;
  value: string;
  isFlipped: boolean;
  isMatched: boolean;
}

export enum GameStatus {
  NotStarted = 0,
  InProgress = 1,
  Completed = 2
}

export interface Game {
  id: string;
  cards: Card[];
  status: GameStatus;
  moves: number;
  matchedPairs: number;
  createdAt: string;
  completedAt?: string;
  duration?: string;
  durationFormatted?: string;
  isGameComplete: boolean;
}

export interface CreateGameRequest {
  gridSize: number;
}

export interface FlipCardRequest {
  cardId: number;
}