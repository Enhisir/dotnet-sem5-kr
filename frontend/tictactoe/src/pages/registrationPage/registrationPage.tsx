import { useState } from 'react';
import Button from '../../components/button/button.tsx';
import Input from '../../components/input/input.tsx';
import classes from './registrationPage.module.css';
import {useNavigate} from "react-router-dom";

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

  const navigator = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (form.password !== form.confirmPassword) {
      return;
    }
    console.log('Регистрация:', form);
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
          <Button type="submit">Регистрация</Button>
        </form>
      </div>
      <div className={classes.container}>
        <Button onClick={() => navigator("/login")}>Вход</Button>
      </div>
    </>
  );
};

export default RegistrationPage;
