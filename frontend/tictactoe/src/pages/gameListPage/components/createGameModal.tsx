import React, { useState } from 'react';
import styles from '../gameListPage.module.css';
import Input from '../../../components/input/input.tsx';
import Button from '../../../components/button/button.tsx';
import api from '../../../client/axiosInstance.ts';
import {useNavigate} from "react-router-dom";

const CreateGameModal = ({ onClose }) => {
  const [maxRating, setMaxRating] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleCreate = () => {
    if (!maxRating || isNaN(Number(maxRating)) || Number(maxRating) <= 0) {
      setError('Введите корректное значение рейтинга');
      return;
    }

    api.post('/games/create', { maxRating: Number(maxRating) })
      .then(response => {
        const { id } = response.data;
        navigate(`/game/${id}`);
      })
      .catch((error) => {
        console.log(error)
        setError('Ошибка при создании игры');
      });
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <button className={styles.closeButton} onClick={onClose}>&times;</button>
        <h2 className={styles.modalTitle}>Создание игры:</h2>
        <p className={styles.maxRating}>Введите максимальный рейтинг</p>
        <Input
          type="text"
          placeholder="Макс. рейтинг"
          value={maxRating}
          onChange={(e) => {
            setMaxRating(e.target.value);
            setError(null);
          }}
        />
        {error && <div className={styles.error}>{error}</div>}
        <Button onClick={handleCreate}>Создать игру</Button>
      </div>
    </div>
  );
};

export default CreateGameModal;
