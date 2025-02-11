import React from 'react';
import styles from './gameLog.module.css';

interface LogEntry {
  id: string;
  timestamp: Date;
  message: string;
  type: 'move' | 'join' | 'leave' | 'system';
  player?: string;
  gameState?: 'waiting' | 'running' | 'finished';
}

interface GameLogProps {
  log: LogEntry[];
}

const GameLog: React.FC<GameLogProps> = ({ log }) => {
  const formatTimestamp = (date: Date) => {
    return date.toLocaleTimeString('ru-RU', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  };

  return (
    <div className={styles.log}>
      {log.map(entry => (
        <div
          key={entry.id}
          className={`${styles.logEntry} ${styles[entry.type]}`}
        >
          <span className={styles.timestamp}>
            [{formatTimestamp(entry.timestamp)}]
          </span>
          <span className={styles.message}>{entry.message}</span>
        </div>
      ))}
    </div>
  );
};

export default GameLog;