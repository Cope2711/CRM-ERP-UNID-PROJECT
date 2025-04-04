﻿using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class AuthControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/auth";

    public AuthControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class LoginTests : AuthControllerTests
    {
        public LoginTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> LoginTestData()
        {
            yield return new object[]
            {
                new LoginUserDto
                {
                    UserUserName = Models.Users.TestUser.UserUserName,
                    UserPassword = "123456",
                    DeviceId = "1"
                },
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                new LoginUserDto
                {
                    UserUserName = "non-existent-user",
                    UserPassword = "123456",
                    DeviceId = "1"
                },
                HttpStatusCode.NotFound
            };

            yield return new object[]
            {
                new LoginUserDto
                {
                    UserUserName = "admin",
                    UserPassword = "invalid-password",
                    DeviceId = "1"
                },
                HttpStatusCode.Unauthorized
            };
        }

        [Theory]
        [MemberData(nameof(LoginTestData))]
        public async Task Login_ReturnsExpectedResult(LoginUserDto loginUserDto, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/login", loginUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }

        [Fact]
        public async Task Login_WhenDevicesReachedTheMax_ReturnsUnauthorized()
        {
            for (int device = 0; device < 3; device++)
            {
                LoginUserDto loginUserDto = new LoginUserDto
                {
                    UserUserName = Models.Users.Admin.UserUserName,
                    UserPassword = "123456",
                    DeviceId = device.ToString()
                };

                var response = await _client.PostAsJsonAsync($"{Endpoint}/login", loginUserDto);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            LoginUserDto loginUserDtoFinal = new LoginUserDto
            {
                UserUserName = Models.Users.Admin.UserUserName,
                UserPassword = "123456",
                DeviceId = "1231241241241"
            };

            var response2 = await _client.PostAsJsonAsync($"{Endpoint}/login", loginUserDtoFinal);
            response2.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    public class RefreshTokenTests : AuthControllerTests
    {
        public RefreshTokenTests(CustomWebApiFactory factory) : base(factory)
        {
        }


        [Fact]
        public async Task RefreshToken_WhenRefreshTokenIsValid_ReturnsTokenDto()
        {
            // Arrange
            LoginUserDto loginUserDto = new LoginUserDto
            {
                UserUserName = "admin",
                UserPassword = "123456",
                DeviceId = "devicexd1"
            };

            // Act
            var response = await _client.PostAsJsonAsync($"{Endpoint}/login", loginUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            tokenDto.Should().NotBeNull();
            tokenDto.Token.Should().NotBeNullOrEmpty();
            tokenDto.RefreshToken.Should().NotBeNullOrEmpty();

            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = tokenDto.RefreshToken,
                DeviceId = "devicexd1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/refresh-token", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var refreshTokenDto = await refreshTokenResponse.Content.ReadFromJsonAsync<TokenDto>();
            refreshTokenDto.Should().NotBeNull();
            refreshTokenDto.Token.Should().NotBeNullOrEmpty();
            refreshTokenDto.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task RefreshToken_WhenDeviceIdIsInvalid_ReturnsUnauthorized()
        {
            // Arrange
            LoginUserDto loginUserDto = new LoginUserDto
            {
                UserUserName = "admin",
                UserPassword = "123456",
                DeviceId = "devicexd1"
            };

            // Act
            var response = await _client.PostAsJsonAsync($"{Endpoint}/login", loginUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
            tokenDto.Should().NotBeNull();
            tokenDto.Token.Should().NotBeNullOrEmpty();
            tokenDto.RefreshToken.Should().NotBeNullOrEmpty();

            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = tokenDto.RefreshToken,
                DeviceId = "devicexd11"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/refresh-token", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_WhenRefreshTokenIsInvalid_ReturnsNotFound()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = "invalid-refresh-token",
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RefreshToken_WhenRefreshTokenIsExpired_ReturnsUnauthorized()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = Models.RefreshTokens.TestUserExpiredRefreshToken.Token,
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_WhenRefreshTokenIsRevoked_ReturnsUnauthorized()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = Models.RefreshTokens.TestUserRefreshTokenRevoked.Token,
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    public class LogoutTests : AuthControllerTests
    {
        public LogoutTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Logout_WhenRefreshTokenIsValid_ReturnsTokenDto()
        {
            // Logg in
            var loginResponse = await _client.PostAsJsonAsync($"{Endpoint}/login", new LoginUserDto
            {
                UserUserName = Models.Users.TestUser.UserUserName,
                UserPassword = "123456",
                DeviceId = "1"
            });

            loginResponse.Should().NotBeNull();
            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            TokenDto? tokenDto = await loginResponse.Content.ReadFromJsonAsync<TokenDto>();

            tokenDto.Should().NotBeNull();
            tokenDto.Token.Should().NotBeNullOrEmpty();
            tokenDto.RefreshToken.Should().NotBeNullOrEmpty();

            // Logout
            var logoutResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", new RefreshTokenEntryDto
            {
                RefreshToken = tokenDto.RefreshToken,
                DeviceId = "1"
            });

            logoutResponse.Should().NotBeNull();
            logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            TokenDto? logoutTokenDto = await logoutResponse.Content.ReadFromJsonAsync<TokenDto>();

            logoutTokenDto.Should().NotBeNull();
            logoutTokenDto.Token.Should().BeNull();
            logoutTokenDto.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Logout_WhenRefreshTokenIsInvalid_ReturnsNotFound()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = "invalid-refresh-token",
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Logout_WhenRefreshTokenIsExpired_ReturnsUnauthorized()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = Models.RefreshTokens.TestUserRefreshTokenRevoked.Token,
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Logout_WhenRefreshTokenIsRevoked_ReturnsUnauthorized()
        {
            // Arrange
            RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
            {
                RefreshToken = Models.RefreshTokens.TestUserRefreshTokenRevoked.Token,
                DeviceId = "1"
            };

            // Act
            var refreshTokenResponse = await _client.PostAsJsonAsync($"{Endpoint}/logout", refreshTokenEntryDto);

            // Assert
            refreshTokenResponse.Should().NotBeNull();
            refreshTokenResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    public class RequestResetTests : AuthControllerTests
    {
        public RequestResetTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> RequestResetTestData()
        {
            yield return new object[]
            {
                new RequestPasswordResetDto
                {
                    Email = "admin@admin.com" // Correo electrónico válido
                },
                HttpStatusCode.OK
            };
            
            yield return new object[]
            {
                new RequestPasswordResetDto
                {
                    Email = "invalid-email@example.com" // Correo electrónico inválido
                },
                HttpStatusCode.NotFound
            };
        }
        
        [Theory]
        [MemberData(nameof(RequestResetTestData))]
        public async Task RequestResetAsync_ReturnsExpectedResult(RequestPasswordResetDto requestPasswordResetDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/request-reset", requestPasswordResetDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ResetPasswordTests : AuthControllerTests
    {
        public ResetPasswordTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> ResetPasswordTestData()
        {
            yield return new object[]
            {
                // Password does not match
                new ResetPasswordDto
                {
                    Token = Models.PasswordRecoveryTokens.TestValidTokenAsync.ResetToken,
                    Email = Models.Users.TestUser.UserEmail,
                    NewPassword = "1234565557",
                    ConfirmPassword = "123458888"
                },
                HttpStatusCode.BadRequest
            };
            
            // All ok :)
            yield return new object[]
            {
                new ResetPasswordDto
                {
                    Token = Models.PasswordRecoveryTokens.TestValidTokenAsync.ResetToken, // Token válido
                    Email = Models.Users.TestUser.UserEmail,
                    NewPassword = "newPassword123",
                    ConfirmPassword = "newPassword123"
                },
                HttpStatusCode.OK
            };
            
            // Token not found
            yield return new object[]
            {
                new ResetPasswordDto
                {
                    Token = "invalid-token", // Token inválido
                    Email = Models.Users.TestUser.UserEmail,
                    NewPassword = "newPassword123",
                    ConfirmPassword = "newPassword123"
                },
                HttpStatusCode.NotFound
            };
            
            // Token expired
            yield return new object[]
            {
                new ResetPasswordDto
                {
                    Token = Models.PasswordRecoveryTokens.TestExpiredTokenAsync.ResetToken, // Token expirado
                    Email = Models.Users.TestUser.UserEmail,
                    NewPassword = "newPassword123",
                    ConfirmPassword = "newPassword123"
                },
                HttpStatusCode.BadRequest
            };
        }
        
        [Theory]
        [MemberData(nameof(ResetPasswordTestData))]
        public async Task ResetPassword_ReturnsExpectedResult(ResetPasswordDto resetPasswordDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/reset-password", resetPasswordDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}