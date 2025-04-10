import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import { Provider } from 'react-redux';
import { store } from '@/redux/store.ts';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from '@/pages/Login.tsx';
import DynamicCreateRolePage from "@/pages/TestsDynamicCreation.tsx";
import App from './App';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <Router>
        <Routes>
          <Route path="/" element={<Navigate to="/login" />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/test1" element={<DynamicCreateRolePage />} />
          <Route path="/app/*" element={<App />} />
        </Routes>
      </Router>
    </Provider>
  </StrictMode>
);
