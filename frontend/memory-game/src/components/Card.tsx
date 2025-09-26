import React from 'react';
import { Card as CardType } from '../types/game';
import './Card.css';

interface CardProps {
  card: CardType;
  onCardClick: (cardId: number) => void;
  disabled?: boolean;
}

const Card: React.FC<CardProps> = ({ card, onCardClick, disabled = false }) => {
  const handleClick = () => {
    if (!disabled && !card.isFlipped && !card.isMatched) {
      onCardClick(card.id);
    }
  };

  return (
    <div
      className={`card ${card.isFlipped || card.isMatched ? 'flipped' : ''} ${
        card.isMatched ? 'matched' : ''
      } ${disabled ? 'disabled' : ''}`}
      onClick={handleClick}
    >
      <div className="card-inner">
        <div className="card-front">
          ?
        </div>
        <div className="card-back">
          {card.value}
        </div>
      </div>
    </div>
  );
};

export default Card;