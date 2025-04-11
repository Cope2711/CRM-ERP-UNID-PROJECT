import { useDispatch, useSelector } from 'react-redux';
import { selectAuth } from '@/redux/auth/selectors';
import { useNavigate } from 'react-router-dom';
import { AppDispatch } from '@/redux/store';
import {logoutThunk} from "@/redux/auth/thunks.ts";
import {logout} from "@/redux/auth/authSlice.ts";
import {RefreshTokenEntryDto} from "@/dtos/AuthDtos.ts";

export const useLogout = () => {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const { refreshToken } = useSelector(selectAuth);

    const handleLogout = async () => {
        if (refreshToken) {
            const payload: RefreshTokenEntryDto = {
                refreshToken: refreshToken,
                deviceId: 'some-device-id',
            };

            await dispatch(logoutThunk(payload));
        }

        dispatch(logout());
        navigate('/login');
    };

    return handleLogout;
};