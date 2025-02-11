import React from 'react';
import styles from '../gameListPage.module.css';
import Button from "../../../components/button/button.tsx";

interface GameCardProps {
  game: {
    id: number;
    username: string;
    date: string;
    status: string;
    ratingAllowed: boolean;
  };
}

const GameCard: React.FC<GameCardProps> = ({ game }) => {
  return (
    <div className={styles.card}>
      <div className={styles.gameInfo}>
        <p className={styles.gameDetail}><strong>Идентификатор игры:</strong> {game.id}</p>
        <p className={styles.gameDetail}><strong>Имя пользователя:</strong> {game.username}</p>
        <p className={styles.gameDetail}><strong>Дата создания:</strong> {game.date}</p>
        <p className={styles.gameDetail}><strong>Статус:</strong> {game.status}</p>
      </div>
      <Button
        className={styles.joinButton}
        disabled={!game.ratingAllowed}
      >
        Зайти в комнату
      </Button>
    </div>
  );
};

export default GameCard;