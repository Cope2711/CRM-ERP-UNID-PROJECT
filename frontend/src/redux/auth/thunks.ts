import { createAsyncThunk } from '@reduxjs/toolkit';
import { LoginRequestDto, LoginResponseDto } from '@/dtos/AuthDtos.ts';
import authService from '@/services/authService';
import { ErrorDetail } from '@/dtos/ErrorDetailDtos.ts';

export const login =
  createAsyncThunk<
    LoginResponseDto,
    LoginRequestDto,
    {
      rejectValue: ErrorDetail;
    }
  >(
    'auth/login',
    async (payload, thunkApi) => {
      try {
        const data = await authService.login(payload);
        return data;
      } catch (error: any) {
        return thunkApi.rejectWithValue(error);
      }
    }
  );
