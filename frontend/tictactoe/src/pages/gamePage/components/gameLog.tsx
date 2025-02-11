import React from 'react';
import styles from './gameLog.module.css';

interface GameLogProps {
  log: string[];
}

const GameLog: React.FC<GameLogProps> = ({ log }) => {
  return (
    <div className={styles.log}>
      {log.length > 0 && (
        <div className={styles.logContent}>
          {log.map((entry, index) => (
            <div key={index} className={styles.logEntry}>{entry}</div>
          ))}
        </div>
      )}
    </div>
  );
};

export default GameLog;