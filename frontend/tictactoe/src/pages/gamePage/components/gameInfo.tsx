import React from 'react';
import styles from './gameInfo.module.css';

interface GameInfoProps {
  currentPlayer: string;
}

const GameInfo: React.FC<GameInfoProps> = ({ currentPlayer }) => {
  return <div className={styles.info}>Ход игрока: {currentPlayer}</div>;
};

export default GameInfo;