import React from 'react';
import styles from './cell.module.css';

interface CellProps {
  value: string | null;
  onClick: () => void;
}

const Cell: React.FC<CellProps> = ({ value, onClick }) => {
  return (
    <div className={styles.cell} onClick={onClick}>
      {value}
    </div>
  );
};

export default Cell;