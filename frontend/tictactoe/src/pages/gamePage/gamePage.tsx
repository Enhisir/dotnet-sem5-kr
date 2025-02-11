import React, { useState, useEffect } from 'react';
import GameBoard from './components/GameBoard';
import GameLog from './components/GameLog';
import styles from './gamePage.module.css';
import { useProfile } from "../../contexts/profileContext.tsx";
import { Navigate, useParams } from "react-router-dom";
import { signalRService } from '../../signalr/signalRService';

const GamePage: React.FC = () => {
  const [currentPlayer, setCurrentPlayer] = useState<'X' | 'O'>('X');
  const [log, setLog] = useState<string[]>([]);
  const [board, setBoard] = useState<(string | null)[]>(Array(9).fill(null));
  const [isJoined, setIsJoined] = useState(false);
  const [profile, loading] = useProfile();
  const { gameRoomId } = useParams();

  useEffect(() => {
    if (profile && gameRoomId) {
      signalRService.connect(gameRoomId);
      signalRService.on('ReceiveMove', handleReceiveMove);
      signalRService.on('PlayerJoined', handlePlayerJoined);
      signalRService.on('PlayerLeft', handlePlayerLeft);
      signalRService.on('GameStarted', handleGameStarted);
      signalRService.on('GameEnded', handleGameEnded);
    }

    return () => {
      signalRService.disconnect();
    };
  }, [profile, gameRoomId]);

  if (profile == null && !loading) return <Navigate to="/login" />;

  const handleMove = (cellIndex: number) => {
    if (!board[cellIndex] && isJoined) {
      signalRService.send('PlayerMadeTurn', cellIndex);
    }
  };

  const handleReceiveMove = (data: { cellIndex: number; player: string }) => {
    setBoard((prevBoard) => {
      const newBoard = [...prevBoard];
      newBoard[data.cellIndex] = data.player;
      return newBoard;
    });
    setLog((prevLog) => [...prevLog, `Игрок ${data.player} сделал ход в клетку ${data.cellIndex + 1}`]);
    setCurrentPlayer((prev) => (prev === 'X' ? 'O' : 'X'));
  };

  const handlePlayerJoined = (player: string) => {
    setLog((prevLog) => [...prevLog, `${player} присоединился к игре`]);
  };

  const handlePlayerLeft = (player: string) => {
    setLog((prevLog) => [...prevLog, `${player} покинул игру`]);
  };

  const handleGameStarted = () => {
    setLog((prevLog) => [...prevLog, 'Игра началась!']);
  };

  const handleGameEnded = (winner: string | null) => {
    setLog((prevLog) => [...prevLog, winner ? `Игрок ${winner} победил!` : 'Ничья!']);
  };

  const handleJoinLeave = async () => {
    if (isJoined) {
      await signalRService.send('LeaveGameRoom');
    } else {
      await signalRService.send('EnterGameRoom');
    }
    setIsJoined((prev) => !prev);
  };

  return (
    <div className={styles.container}>
      <div className={styles.boardContainer}>
        <div className={styles.currentPlayer}>Ход игрока: {currentPlayer}</div>
        <GameBoard currentPlayer={currentPlayer} board={board} onMove={handleMove} />
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