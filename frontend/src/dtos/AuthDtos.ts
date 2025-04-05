export interface LoginRequestDto {
  UserUserName: string;
  UserPassword: string;
  DeviceId: string;
}

export interface LoginResponseDto {
  token: string;
  refreshToken: string;
}
