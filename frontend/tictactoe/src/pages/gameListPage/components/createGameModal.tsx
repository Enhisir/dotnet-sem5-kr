import React, { useState } from 'react';
import styles from '../gameListPage.module.css';
import Input from "../../../components/input/input.tsx";
import Button from "../../../components/button/button.tsx";

const CreateGameModal = ({ onClose, onCreate }) => {
  const [maxRating, setMaxRating] = useState('');

  const handleCreate = () => {
    onCreate(maxRating);
    onClose();
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
          onChange={(e) => setMaxRating(e.target.value)}
        />
        <Button onClick={handleCreate}>Создать игру</Button>
      </div>
    </div>
  );
};

export default CreateGameModal;