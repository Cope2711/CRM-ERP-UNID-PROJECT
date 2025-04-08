import {StrictMode} from 'react';
import {createRoot} from 'react-dom/client';
import './index.css';
import App from './App.tsx';
import {Provider} from 'react-redux';
import {store} from '@/redux/store.ts';
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom';
import LoginPage from '@/pages/Login.tsx';
import DynamicCreateRolePage from "@/pages/TestsDynamicCreation.tsx";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <Provider store={store}>
            <Router>
                <Routes>
                    <Route path="/" element={<App/>}/>
                    <Route path="/login" element={<LoginPage/>}/>
                    <Route path="/test1" element={<DynamicCreateRolePage/>}/>
                </Routes>
            </Router>
        </Provider>
    </StrictMode>,
);
