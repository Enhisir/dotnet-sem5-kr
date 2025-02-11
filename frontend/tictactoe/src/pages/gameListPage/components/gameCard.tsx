import React from 'react';
import styles from '../gameListPage.module.css';
import Button from "../../../components/button/button.tsx";
import { useNavigate } from "react-router-dom";

interface GameCardProps {
  game: {
    id: string;
    players: string[];
    createdAt: string;
    state: number;
    ratingAllowed: boolean;
  };
}

const GameCard: React.FC<GameCardProps> = ({ game }) => {
  const navigate = useNavigate();

  const handleJoinGame = () => {
    navigate(`/game/${game.id}`);
  };

  const formattedDate = new Date(game.createdAt).toLocaleString('ru-RU', {
    day: '2-digit', month: '2-digit', year: 'numeric'
  });

  return (
    <div className={styles.card}>
      <div className={styles.gameInfo}>
        <p className={styles.gameDetail}><strong>Идентификатор игры:</strong> {game.id}</p>
        <p className={styles.gameDetail}><strong>Имя пользователя:</strong> {game.players.join(', ')}</p>
        <p className={styles.gameDetail}><strong>Дата создания:</strong> {formattedDate}</p>
        <p className={styles.gameDetail}><strong>Статус:</strong> {game.state}</p>
      </div>
      <Button
        className={styles.joinButton}
        disabled={!game.ratingAllowed}
        onClick={handleJoinGame}
      >
        Зайти в комнату
      </Button>
    </div>
  );
};

export default GameCard;