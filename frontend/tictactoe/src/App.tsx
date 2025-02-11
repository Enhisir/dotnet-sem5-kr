import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RegistrationPage  from './pages/registrationPage/registrationPage.tsx';
import LoginPage from './pages/loginPage/loginPage.tsx';
import GameListPage from './pages/gameListPage/gameListPage.tsx';
import GamePage from './pages/gamePage/gamePage.tsx';
import "./App.css";


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/register" element={<RegistrationPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/games" element={<GameListPage />} />
        <Route path="/game/:id" element={<GamePage />} />
        <Route path="*"  element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;
