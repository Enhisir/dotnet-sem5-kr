import React, { useState } from 'react';
import Button from '../../components/button/button.tsx';
import Input from '../../components/input/input.tsx';
import classes from './registrationPage.module.css';
import {Navigate, useNavigate} from 'react-router-dom';
import api from '../../client/axiosInstance.ts';
import {useProfile} from "../../contexts/profileContext.tsx";

interface FormState {
  username: string;
  password: string;
  confirmPassword: string;
}

const RegistrationPage = () => {
  const [form, setForm] = useState<FormState>({
    username: '',
    password: '',
    confirmPassword: '',
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();
  const [profile, loading] = useProfile();
  if (profile != null) return <Navigate to="/games"></Navigate>;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    setError(null);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (form.password !== form.confirmPassword) {
      setError('Пароли не совпадают');
      return;
    }

    const payload = {
      userName: form.username,
      password: form.password,
    };

    api.post('/auth/sign_up', payload)
      .then(() => navigate('/login'))
      .catch((err) => {
        if (err.response && err.response.data) {
          setError(err.response.data.message || 'Ошибка регистрации');
        } else {
          setError('Ошибка регистрации');
        }
      });
  };

  return (
    <>
      <div className={classes.container}>
        <h1 className={classes.title}>Крестики-нолики</h1>
        <h2 className={classes.subtitle}>Регистрация</h2>
        <form onSubmit={handleSubmit}>
          <Input
            type="text"
            name="username"
            placeholder="Логин"
            value={form.username}
            onChange={handleChange}
          />
          <Input
            type="password"
            name="password"
            placeholder="Пароль"
            value={form.password}
            onChange={handleChange}
          />
          <Input
            type="password"
            name="confirmPassword"
            placeholder="Повторите пароль"
            value={form.confirmPassword}
            onChange={handleChange}
          />
          {error && <div className={classes.error}>{error}</div>}
          <Button type="submit">Регистрация</Button>
        </form>

      </div>
      <div className={classes.container}>
        <Button onClick={() => navigate('/login')}>Вход</Button>
      </div>
    </>
  );
};

export default RegistrationPage;
