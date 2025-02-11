import React from 'react';
import styles from '../gameListPage.module.css';

interface RatingModalProps {
  isOpen: boolean;
  onClose: () => void;
  username?: string;
  rating?: number;
}

const RatingModal: React.FC<RatingModalProps> = (
  {
    isOpen,
    onClose,
    username,
    rating
  }) => {
  if (!isOpen) return null;

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalContent}>
        <button className={styles.closeButton} onClick={onClose}>
          &times;
        </button>
        <div className={styles.modalTitle}>Ваша информация</div>
        <div className={styles.modalText}>Ник: {username}</div>
        <div className={styles.modalText}>Рейтинг: {rating}</div>
      </div>
    </div>
  );
};

export default RatingModal;