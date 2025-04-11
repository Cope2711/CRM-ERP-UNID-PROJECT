import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

export const useRedirectIfAuthenticated = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const token = localStorage.getItem('access_token');
        if (token) {
            navigate('/app/dashboard');
        }
    }, [navigate]);
};