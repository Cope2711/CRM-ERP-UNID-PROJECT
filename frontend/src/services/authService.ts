import { LoginRequestDto, LoginResponseDto } from '@/dtos/AuthDtos.ts';
import axiosInstance from '@/services/axiosConfig';
import { ErrorDetail } from '@/dtos/ErrorDetailDtos.ts';

class authService {
  async login(payload: LoginRequestDto): Promise<LoginResponseDto> {
    try {
      const { data } = await axiosInstance.post('auth/login', payload);
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
}

export default new authService();
