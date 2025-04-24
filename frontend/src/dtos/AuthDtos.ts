import { UserDto } from "./UserDtos";

export interface LoginRequestDto {
  UserUserName: string;
  UserPassword: string;
  DeviceId: string;
}

export interface LoginResponseDto {
  token: string;
  refreshToken: string;
  user: UserDto;
}

export interface RefreshTokenEntryDto {
  refreshToken: string;
  deviceId: string;
}