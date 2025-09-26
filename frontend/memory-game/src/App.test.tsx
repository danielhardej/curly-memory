import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import App from './App';

// Mock the fetch API since we'll be making API calls
const mockFetch = jest.fn();
global.fetch = mockFetch;

beforeEach(() => {
  mockFetch.mockClear();
  // Mock successful game creation
  mockFetch.mockResolvedValue({
    ok: true,
    json: async () => ({
      id: 'test-game-id',
      cards: [
        { id: 0, value: '🐶', isFlipped: false, isMatched: false },
        { id: 1, value: '🐶', isFlipped: false, isMatched: false },
        { id: 2, value: '🐱', isFlipped: false, isMatched: false },
        { id: 3, value: '🐱', isFlipped: false, isMatched: false }
      ],
      status: 1, // InProgress
      moves: 0,
      matchedPairs: 0,
      createdAt: new Date().toISOString(),
      isGameComplete: false
    })
  });
});

test('renders memory game title', async () => {
  render(<App />);
  await waitFor(() => {
    const titleElement = screen.getByText(/memory game/i);
    expect(titleElement).toBeInTheDocument();
  });
});

test('renders new game button', async () => {
  render(<App />);
  await waitFor(() => {
    const buttonElement = screen.getByText(/new game/i);
    expect(buttonElement).toBeInTheDocument();
  });
});

test('renders grid size selector', async () => {
  render(<App />);
  await waitFor(() => {
    const selectorElement = screen.getByLabelText(/grid size/i);
    expect(selectorElement).toBeInTheDocument();
  });
});
