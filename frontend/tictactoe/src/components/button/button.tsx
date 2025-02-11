import React from 'react';
import styles from './button.module.css';

interface ButtonProps {
  className?: string;
  disabled?: boolean;
<<<<<<< HEAD
=======
  className?: string;
  disabled?: boolean;
>>>>>>> main
  children: React.ReactNode;
  onClick?: () => void;
  type?: 'button' | 'submit' | 'reset';
}

const Button: React.FC<ButtonProps> = (
  {
    className,
    children,
    onClick,
    type = 'button',
  }) => {
  return (
    <button className={`${styles.btn} ${className}`} onClick={onClick} type={type}>
      {children}
    </button>
  );
};

export default Button;
