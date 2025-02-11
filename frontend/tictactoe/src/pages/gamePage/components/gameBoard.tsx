import React, { useState } from 'react';
import Cell from './Cell';
import styles from './gameBoard.module.css';

interface GameBoardProps {
  currentPlayer: string;
  onMove: (index: number) => void;
}

const GameBoard: React.FC<GameBoardProps> = ({ currentPlayer, onMove }) => {
  const [board, setBoard] = useState<(string | null)[]>(Array(9).fill(null));

  const handleClick = (index: number) => {
    if (!board[index]) {
      const newBoard = [...board];
      newBoard[index] = currentPlayer;
      setBoard(newBoard);
      onMove(index);
    }
  };

  return (
    <div className={styles.board}>
      {board.map((value, index) => (
        <Cell key={index} value={value} onClick={() => handleClick(index)} />
      ))}
    </div>
  );
};

export default GameBoard;