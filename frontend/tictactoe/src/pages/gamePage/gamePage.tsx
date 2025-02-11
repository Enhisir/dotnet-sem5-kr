import React, { useState, useEffect } from 'react';
import GameBoard from './components/GameBoard';
import GameLog from './components/GameLog';
import styles from './gamePage.module.css';
import { useProfile } from "../../contexts/profileContext.tsx";
import {Navigate, useNavigate, useParams} from "react-router-dom";
import { signalRService } from '../../signalr/signalRService';

interface LogEntry {
  id: string;
  timestamp: Date;
  message: string;
  type: 'move' | 'join' | 'leave' | 'system';
  player?: string;
  gameState?: 'waiting' | 'running' | 'finished';
}

const GamePage: React.FC = () => {
  const [currentPlayer, setCurrentPlayer] = useState<'X' | 'O'>('X');
  const [log, setLog] = useState<LogEntry[]>([]);
  const [board, setBoard] = useState<(string | null)[]>(Array(9).fill(null));
  const [isJoined, setIsJoined] = useState(false);
  const [gameState, setGameState] = useState<'waiting' | 'running' | 'finished'>('waiting');
  const [players, setPlayers] = useState<string[]>([]);
  const [profile, loading] = useProfile();
  const { gameRoomId } = useParams();
  const navigate = useNavigate();

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
    setGameState('running')
    console.log(board, isJoined)
    if (!board[cellIndex] && isJoined) {
      const x = cellIndex % 3;
      const y = Math.floor(cellIndex / 3);
      signalRService.send('PlayerMadeTurn', x, y);
    }
  };

  const handleReceiveMove = (data: { x: number; y: number; player: string }) => {
    const cellIndex = data.y * 3 + data.x;
    setBoard(prev => {
      const newBoard = [...prev];
      newBoard[cellIndex] = data.player;
      return newBoard;
    });

    setLog(prev => [
      ...prev,
      {
        id: Math.random().toString(),
        timestamp: new Date(),
        message: `Игрок ${data.player} сделал ход в клетку (${data.x + 1}, ${data.y + 1})`,
        type: 'move',
        player: data.player
      }
    ]);
  };

  const handlePlayerJoined = (player: string) => {
    setPlayers(prev => [...prev, player]);
    setLog(prev => [
      ...prev,
      {
        id: Math.random().toString(),
        timestamp: new Date(),
        message: `${player} присоединился к игре`,
        type: 'join',
        player
      }
    ]);
  };

  const handlePlayerLeft = (player: string) => {
    setPlayers(prev => prev.filter(p => p !== player));
    setLog(prev => [
      ...prev,
      {
        id: Math.random().toString(),
        timestamp: new Date(),
        message: `${player} покинул игру`,
        type: 'leave',
        player
      }
    ]);
  };

  const handleGameStarted = () => {
    setGameState('running');
    setLog(prev => [
      ...prev,
      {
        id: Math.random().toString(),
        timestamp: new Date(),
        message: 'Игра началась!',
        type: 'system',
        gameState: 'running'
      }
    ]);
  };

  const handleGameEnded = (winner: string | null) => {
    setGameState('finished');
    setLog(prev => [
      ...prev,
      {
        id: Math.random().toString(),
        timestamp: new Date(),
        message: winner ? `Победил ${winner}!` : 'Ничья!',
        type: 'system',
        gameState: 'finished'
      }
    ]);

    setTimeout(() => {
      setBoard(Array(9).fill(null));
      setGameState('waiting');
    }, 3000);
  };

  const handleJoinLeave = async () => {
    if (isJoined) {
      await signalRService.send('LeaveGameRoomAsync');
      navigate("/games");
    } else {
      await signalRService.send('EnterGameRoomAsync');
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
          {/*<div>Игрок 1: {players[0] || 'ожидание'}</div>
          <div>Игрок 2: {players[1] || 'ожидание'}</div>*/}
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