import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Card from './Card';
import { Card as CardType } from '../types/game';

describe('Card Component', () => {
  const mockCard: CardType = {
    id: 1,
    value: '🐶',
    isFlipped: false,
    isMatched: false,
  };

  const mockOnCardClick = jest.fn();

  beforeEach(() => {
    mockOnCardClick.mockClear();
  });

  test('renders card with question mark when not flipped', () => {
    render(<Card card={mockCard} onCardClick={mockOnCardClick} />);
    expect(screen.getByText('?')).toBeInTheDocument();
  });

  test('renders card value when flipped', () => {
    const flippedCard = { ...mockCard, isFlipped: true };
    render(<Card card={flippedCard} onCardClick={mockOnCardClick} />);
    expect(screen.getByText('🐶')).toBeInTheDocument();
  });

  test('calls onCardClick when clicked and not flipped', () => {
    render(<Card card={mockCard} onCardClick={mockOnCardClick} />);
    const cardElement = screen.getByText('?').closest('.card');
    fireEvent.click(cardElement!);
    expect(mockOnCardClick).toHaveBeenCalledWith(1);
  });

  test('does not call onCardClick when card is already flipped', () => {
    const flippedCard = { ...mockCard, isFlipped: true };
    render(<Card card={flippedCard} onCardClick={mockOnCardClick} />);
    const cardElement = screen.getByText('🐶').closest('.card');
    fireEvent.click(cardElement!);
    expect(mockOnCardClick).not.toHaveBeenCalled();
  });

  test('does not call onCardClick when card is matched', () => {
    const matchedCard = { ...mockCard, isMatched: true };
    render(<Card card={matchedCard} onCardClick={mockOnCardClick} />);
    const cardElement = screen.getByText('?').closest('.card');
    fireEvent.click(cardElement!);
    expect(mockOnCardClick).not.toHaveBeenCalled();
  });

  test('does not call onCardClick when disabled', () => {
    render(<Card card={mockCard} onCardClick={mockOnCardClick} disabled />);
    const cardElement = screen.getByText('?').closest('.card');
    fireEvent.click(cardElement!);
    expect(mockOnCardClick).not.toHaveBeenCalled();
  });
});