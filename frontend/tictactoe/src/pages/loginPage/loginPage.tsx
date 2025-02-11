import { useState } from 'react';
import Button from '../../components/button/button';
import Input from '../../components/input/input';
import classes from './loginPage.module.css';
import {useNavigate} from "react-router-dom";

interface LoginFormState {
  username: string;
  password: string;
}

const LoginPage = () => {
  const [form, setForm] = useState<LoginFormState>({
    username: '',
    password: '',
  });

  const navigator = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log('Вход:', form);
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
          <Button type="submit">Войти</Button>
        </form>
      </div>
      <div className={classes.container}>
        <Button onClick={() => navigator("/register")}>Регистрация</Button>
      </div>
    </>
  );
};

export default LoginPage;
