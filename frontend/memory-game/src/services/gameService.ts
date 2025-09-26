import { Game, CreateGameRequest, FlipCardRequest } from '../types/game';

const API_BASE_URL = 'https://localhost:7253/api'; // Default ASP.NET Core HTTPS port

class GameService {
  async createGame(request: CreateGameRequest): Promise<Game> {
    const response = await fetch(`${API_BASE_URL}/game`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(`Failed to create game: ${response.statusText}`);
    }

    return response.json();
  }

  async getGame(gameId: string): Promise<Game> {
    const response = await fetch(`${API_BASE_URL}/game/${gameId}`);

    if (!response.ok) {
      throw new Error(`Failed to get game: ${response.statusText}`);
    }

    return response.json();
  }

  async flipCard(gameId: string, request: FlipCardRequest): Promise<Game> {
    const response = await fetch(`${API_BASE_URL}/game/${gameId}/flip`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(`Failed to flip card: ${response.statusText}`);
    }

    return response.json();
  }

  async getAllGames(): Promise<Game[]> {
    const response = await fetch(`${API_BASE_URL}/game`);

    if (!response.ok) {
      throw new Error(`Failed to get games: ${response.statusText}`);
    }

    return response.json();
  }
}

const gameServiceInstance = new GameService();
export default gameServiceInstance;