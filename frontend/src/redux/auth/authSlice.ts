import { createSlice } from '@reduxjs/toolkit';
import { login } from '@/redux/auth/thunks.ts';

interface AuthState {
  token: string | null;
  refreshToken: string | null;
  isLoading: boolean;
  isSuccess: boolean;
  isError: boolean;
  errorMessage?: string;
}

const initialState: AuthState = {
  token: null,
  refreshToken: null,
  isLoading: false,
  isSuccess: false,
  isError: false,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    logout: (state) => {
      state.token = null;
      state.refreshToken = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => {
        state.isLoading = true;
        state.isError = false;
        state.isSuccess = false;
        state.errorMessage = undefined;
      })
        .addCase(login.fulfilled, (state, action) => {
          const { token, refreshToken } = action.payload;
          localStorage.setItem('access_token', token);
          localStorage.setItem('refresh_token', refreshToken);
          state.isLoading = false;
          state.token = token;
          state.refreshToken = refreshToken;
          state.isSuccess = true;
          state.errorMessage = undefined;
        })
      .addCase(login.rejected, (state, action) => {
        state.isLoading = false;
        state.isError = true;

        if (action.payload) {
          const { status, detail, title, field } = action.payload;
          state.errorMessage = `${title}: ${detail}`;

          if (status === 404 && field === "UserUserName") {
            state.errorMessage = 'Usuario no encontrado';
          } else if (status === 401) {
            state.errorMessage = 'Contraseña incorrecta';
          } else {
            state.errorMessage = 'Error desconocido';
          }
        } else {
          state.errorMessage = 'Error desconocido';
        }
      });
  },
});

export const { logout } = authSlice.actions;
export default authSlice.reducer;
