import React from 'react';
import styles from '../gameListPage.module.css';
import Button from '../../../components/button/button.tsx';

const Header: React.FC<{ onRatingClick: () => void, onCreateGame: () => void }> = ({ onRatingClick, onCreateGame }) => {
  return (
    <div className={styles.header}>
      <Button className={styles.headerButton} onClick={onRatingClick}>Рейтинг</Button>
      <Button className={styles.headerButton} onClick={onCreateGame}>Создать Игру</Button>
    </div>
  );
};

export default Header;
