import React from 'react';
import styles from './input.module.css';

interface InputProps {
  type: string;
  placeholder: string;
  value: string;
  name?: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const Input: React.FC<InputProps> = ({ type, placeholder, value, onChange, name }) => {
  return (
    <input
      className={styles.input}
      type={type}
      placeholder={placeholder}
      value={value}
      name={name}
      onChange={onChange}
    />
);
};

export default Input;
