import React, { useState, useEffect, useRef } from 'react';
import Header from './components/Header';
import GameCard from './components/GameCard';
import RatingModal from './components/RatingModal';
import styles from './gameListPage.module.css';
import CreateGameModal from "./components/createGameModal.tsx";
import { useProfile } from "../../contexts/profileContext.tsx";
import { Navigate, useNavigate } from "react-router-dom";
import api from "../../client/axiosInstance.ts";

const GameListPage: React.FC = () => {
  const [isModalOpen1, setIsModalOpen1] = useState(false);
  const [isModalOpen2, setIsModalOpen2] = useState(false);
  const [profile, loading] = useProfile();
  const [games, setGames] = useState([]);
  const [offset, setOffset] = useState(0);
  const [hasMore, setHasMore] = useState(true);
  const limit = 10;
  const observer = useRef<IntersectionObserver | null>(null);
  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState(false); // Новый флаг загрузки

  const fetchGames = () => {
    if (isLoading || !hasMore) return;
    setIsLoading(true);

    api.get(`/games?offset=${offset}&limit=${limit}`)
      .then(response => {
        const newGames = response.data;
        setGames(prevGames => [...prevGames, ...newGames]);
        setOffset(prevOffset => prevOffset + limit);
        if (newGames.length < limit) setHasMore(false);
      })
      .catch(error => console.error('Ошибка при загрузке игр:', error))
      .finally(() => setIsLoading(false));
  };

  const lastGameElementRef = (node: HTMLDivElement | null) => {
    if (observer.current) observer.current.disconnect();
    observer.current = new IntersectionObserver(entries => {
      if (entries[0].isIntersecting && !isLoading && hasMore) {
        fetchGames();
      }
    });
    if (node) observer.current.observe(node);
  };

  if (profile == null && !loading) return <Navigate to="/login" />;

  const handleOpenModal1 = () => setIsModalOpen1(true);
  const handleCloseModal1 = () => setIsModalOpen1(false);

  const handleOpenModal2 = () => setIsModalOpen2(true);
  const handleCloseModal2 = () => setIsModalOpen2(false);

  return (
    <div className={styles.container}>
      <Header
        onRatingClick={handleOpenModal1}
        onCreateGame={handleOpenModal2}
      />
      <div ref={lastGameElementRef} style={{height: 1}}/>
      <div className={styles.gameList}>
        {games.map((game, index) => (
          <div
            key={game.id}
            ref={index === games.length - 1 ? lastGameElementRef : null}
          >
            <GameCard game={game}/>
          </div>
        ))}
      </div>
      {isModalOpen1 && (
        <RatingModal
          isOpen={true}
          onClose={handleCloseModal1}
          username={profile?.userName}
          rating={profile?.rating}
        />
      )}
      {isModalOpen2 && (
        <CreateGameModal
          isOpen={true}
          onClose={handleCloseModal2}
        />
      )}
    </div>
  );
};

export default GameListPage;