import {createSlice} from '@reduxjs/toolkit';
import {login} from '@/redux/auth/thunks.ts';

interface AuthState {
    token: string | null;
    refreshToken: string | null;
    isLoading: boolean;
    isSuccess: boolean;
    isError: boolean;
    errorMessage?: string;
    errorField?: string;
}

const initialState: AuthState = {
    token: null,
    refreshToken: null,
    isLoading: false,
    isSuccess: false,
    isError: false,
    errorMessage: undefined,
    errorField: undefined,
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
                const {token, refreshToken} = action.payload;
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
                    const { detail, field } = action.payload;
                    state.errorField = field;
                    state.errorMessage = `${detail}`;
                } else {
                    state.errorMessage = 'Error desconocido';
                    state.errorField = undefined;
                }
            });
    },
});

export const {logout} = authSlice.actions;
export default authSlice.reducer;
