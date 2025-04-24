import axios from 'axios';
import {RefreshTokenEntryDto} from "@/dtos/AuthDtos.ts";
import authService from "@/services/authService.ts";


const baseURl = import.meta.env.VITE_API_URL;
if (!baseURl) {
    throw new Error("Missing baseURL");
}
export const axiosInstance = axios.create({
  baseURL: baseURl,
  headers: {
    'Content-Type': 'application/json',
  },
    withCredentials: true,
});

axiosInstance.defaults.withCredentials = true;

axiosInstance.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem('access_token');
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    response => response,
    async error => {
      const originalRequest = error.config;
      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        try {
          const refreshToken = localStorage.getItem('refresh_token');
          if (refreshToken) {
            const payload: RefreshTokenEntryDto = {
              refreshToken,
              deviceId: 'some-device-id',
            };

            const response = await authService.refreshToken(payload);

            const { token, refreshToken: newRefreshToken } = response;
            localStorage.setItem('access_token', token);
            localStorage.setItem('refresh_token', newRefreshToken);

            originalRequest.headers['Authorization'] = `Bearer ${token}`;
            return axiosInstance(originalRequest);
          }
        } catch (err) {
          localStorage.removeItem('access_token');
          localStorage.removeItem('refresh_token');

          alert('Tu sesión ha expirado. Por favor, inicia sesión de nuevo.');
          window.location.href = '/login';
        }
      }

      return Promise.reject(error);
    }
);

export const axiosInstanceAnonymous = axios.create({
    baseURL: baseURl,
    headers: {
        'Content-Type': 'application/json',
    }
});

