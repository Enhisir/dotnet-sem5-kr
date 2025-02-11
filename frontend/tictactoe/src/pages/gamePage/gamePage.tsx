import React, { useState } from 'react';
import GameBoard from './components/GameBoard';
import GameLog from './components/GameLog';
import styles from './gamePage.module.css';
import {useProfile} from "../../contexts/ProfileContext.tsx";
import {Navigate} from "react-router-dom";

const GamePage: React.FC = () => {
  const [currentPlayer, setCurrentPlayer] = useState('X');
  const [log, setLog] = useState<string[]>([]);
  const [isJoined, setIsJoined] = useState(false);
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;

  const handleMove = (cellIndex: number) => {
    setLog((prevLog) => [...prevLog, `Игрок ${currentPlayer} сделал ход в клетку ${cellIndex + 1}`]);
    setCurrentPlayer((prev) => (prev === 'X' ? 'O' : 'X'));
  };

  const handleJoinLeave = () => {
    setIsJoined((prev) => !prev);
  };

  return (
    <div className={styles.container}>
      <div className={styles.boardContainer}>
        <div className={styles.currentPlayer}>Ход игрока: {currentPlayer}</div>
        <GameBoard currentPlayer={currentPlayer} onMove={handleMove} />
        <div className={styles.footer}>
          <div>Максимальный рейтинг: 1500</div>
          <div>Игрок 1: player1</div>
          <div>Игрок 2: {isJoined ? 'player2' : 'не готов'}</div>
          <button className={styles.joinButton} onClick={handleJoinLeave}>
            {isJoined ? 'Покинуть игру' : 'Присоединиться'}
          </button>
        </div>
      </div>
      <GameLog log={log} />
    </div>
  );
};

export default GamePage;