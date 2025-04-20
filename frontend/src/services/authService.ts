import { LoginRequestDto, LoginResponseDto, RefreshTokenEntryDto } from '@/dtos/AuthDtos.ts';
import { axiosInstanceAnonymous } from '@/services/axiosConfig';
import { ErrorDetail } from '@/dtos/ErrorDetailDtos.ts';

class authService {
  async login(payload: LoginRequestDto): Promise<LoginResponseDto> {
    try {
      const { data } = await axiosInstanceAnonymous.post('auth/login', payload);
      return data;
    } catch (error: any) {
      const response = error.response;
      const errorDetail: ErrorDetail = {
        title: response?.data?.title || 'Error',
        status: response?.status || 500,
        detail: response?.data?.detail || 'Error Desconocido',
        field: response?.data?.field,
      };
      throw errorDetail;
    }
  }

  async logout(payload: RefreshTokenEntryDto): Promise<void> {
    try {
      await axiosInstanceAnonymous.post('auth/logout', payload);
    } catch (error: any) {
      const response = error.response;
      const errorDetail: ErrorDetail = {
        title: response?.data?.title || 'Logout Error',
        status: response?.status || 500,
        detail: response?.data?.detail || 'Error desconocido',
      };
      throw errorDetail;
    }
  }

  async refreshToken(payload: RefreshTokenEntryDto): Promise<LoginResponseDto> {
    try {
      const { data } = await axiosInstanceAnonymous.post('auth/refresh-token', payload);
      return data;
    } catch (error: any) {
      const response = error.response;
      const errorDetail: ErrorDetail = {
        title: response?.data?.title || 'Refresh Token Error',
        status: response?.status || 500,
        detail: response?.data?.detail || 'Error desconocido',
      };
      throw errorDetail;
    }
  }
}

export default new authService();
