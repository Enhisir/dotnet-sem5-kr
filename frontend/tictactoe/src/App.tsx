import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RegistrationPage from './pages/registrationPage/registrationPage';
import LoginPage from './pages/loginPage/loginPage';
import GameListPage from './pages/gameListPage/gameListPage';
import GamePage from './pages/gamePage/gamePage';
import { ProfileProvider } from './contexts/profileContext.tsx';
import './App.css'

function App() {
  return (
    <ProfileProvider>
      <Router>
        <Routes>
          <Route path="/register" element={<RegistrationPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/games" element={<GameListPage />} />
          <Route path="/game/:id" element={<GamePage />} />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </Router>
    </ProfileProvider>
  );
}

export default App;