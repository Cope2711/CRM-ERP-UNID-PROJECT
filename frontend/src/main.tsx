import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import { Provider } from 'react-redux';
import { store, persistor } from '@/redux/store.ts'; // Importa el persistor
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from '@/pages/Login.tsx';
import DynamicCreateUserPage from "@/pages/TestsDynamicCreation.tsx";
import App from './App';
import TestGetAllService from "@/pages/TestGetAllService.tsx";
import DynamicUpdateUserPage from './pages/TestDynamicUpdate';
import { PersistGate } from 'redux-persist/integration/react'; // Importa PersistGate

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      {/* Envuélvelo con PersistGate para esperar la persistencia */}
      <PersistGate loading={null} persistor={persistor}>
        <Router>
          <Routes>
            <Route path="/" element={<Navigate to="/login" />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/test1" element={<DynamicCreateUserPage />} />
            <Route path="/test2" element={<TestGetAllService />} />
            <Route path="/test3" element={<DynamicUpdateUserPage userId='2c0180d4-040c-4c00-b8f9-31f7a1e72259'/>} />
            <Route path="/*" element={<App />} />
          </Routes>
        </Router>
      </PersistGate>
    </Provider>
  </StrictMode>
);
