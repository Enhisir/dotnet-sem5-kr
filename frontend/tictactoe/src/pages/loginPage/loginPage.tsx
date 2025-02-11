import React, { useState } from 'react';
import Button from '../../components/button/button';
import Input from '../../components/input/input';
import classes from './loginPage.module.css';
import { Navigate, useNavigate } from 'react-router-dom';
import { useProfile } from '../../contexts/profileContext.tsx';
import api from '../../client/axiosInstance.ts';

interface LoginFormState {
  username: string;
  password: string;
}

const LoginPage = () => {
  const [form, setForm] = useState<LoginFormState>({ username: '', password: '' });
  const [error, setError] = useState<string | null>(null);
  const [profile, loading] = useProfile();
  const navigate = useNavigate();

  if (profile != null) return <Navigate to="/games" />;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
    setError(null);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    api.post('/auth/sign_in', {
      userName: form.username,
      password: form.password,
    })
      .then(response => {
        const { userName, accessToken, accessTokenExpires, refreshToken } = response.data;

        localStorage.setItem('accessToken', accessToken);
        localStorage.setItem('accessTokenExpires', accessTokenExpires.toString());
        localStorage.setItem('refreshToken', refreshToken);

        window.location.reload();

        navigate('/games');
      })
      .catch(err => {
        if (err.response && err.response.data?.message) {
          setError(err.response.data.message);
        } else {
          setError('Неверный логин или пароль');
        }
      });
  };

  return (
    <>
      <div className={classes.container}>
        <h1 className={classes.title}>Крестики-нолики</h1>
        <h2 className={classes.subtitle}>Вход</h2>
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
          {error && <div className={classes.error}>{error}</div>}
          <Button type="submit">Войти</Button>
        </form>
      </div>
      <div className={classes.container}>
        <Button onClick={() => navigate("/register")}>Регистрация</Button>
      </div>
    </>
  );
};

export default LoginPage;
