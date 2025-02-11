import React from 'react';
import Cell from './Cell';
import styles from './gameBoard.module.css';

interface GameBoardProps {
  currentPlayer: string;
  onMove: (index: number) => void;
  board: (string | null)[];
}

const GameBoard: React.FC<GameBoardProps> = ({ currentPlayer, onMove, board }) => {
  return (
    <div className={styles.board}>
      {board.map((value, index) => (
        <Cell key={index} value={value} onClick={() => onMove(index)} />
      ))}
    </div>
  );
};

export default GameBoard;