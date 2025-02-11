import React, { useState } from 'react';
import Header from './components/Header';
import GameCard from './components/GameCard';
import RatingModal from './components/RatingModal';
import styles from './gameListPage.module.css';
import CreateGameModal from "./components/createGameModal.tsx";

const games = [
  { id: 1, username: 'player1', date: '10.02.2025', status: 'ожидание', ratingAllowed: true },
  { id: 2, username: 'player1', date: '09.02.2025', status: 'ожидание', ratingAllowed: false },
  { id: 3, username: 'player1', date: '10.02.2025', status: 'начата', ratingAllowed: true },
];

const GameListPage: React.FC = () => {
  const [isModalOpen1, setIsModalOpen1] = useState(false);
  const [isModalOpen2, setIsModalOpen2] = useState(false);

  const handleOpenModal1 = () => {
    setIsModalOpen1(true);
  };

  const handleCloseModal1 = () => {
    setIsModalOpen1(false);
  };

  const handleOpenModal2 = () => {
    setIsModalOpen2(true);
  };

  const handleCloseModal2 = () => {
    setIsModalOpen2(false);
  };

  return (
    <div className={styles.container}>
      <Header
        onRatingClick={handleOpenModal1}
        onCreateGame={handleOpenModal2}
      />
      <div className={styles.gameList}>
        {games.map((game) => (
          <GameCard key={game.id} game={game} />
        ))}
      </div>
      {isModalOpen1 && <RatingModal isOpen={true} onClose={handleCloseModal1} />}
      {isModalOpen2 && <CreateGameModal isOpen={true} onClose={handleCloseModal2} />}
    </div>
  );
};

export default GameListPage;