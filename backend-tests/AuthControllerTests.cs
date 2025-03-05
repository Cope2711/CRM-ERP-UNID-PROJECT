using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class AuthControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;

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
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);
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
                
                var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
            
            LoginUserDto loginUserDtoFinal = new LoginUserDto
            {
                UserUserName = Models.Users.Admin.UserUserName,
                UserPassword = "123456",
                DeviceId = "1231241241241"
            };
                
            var response2 = await _client.PostAsJsonAsync("/api/auth/login", loginUserDtoFinal);
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
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenEntryDto);

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
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenEntryDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginUserDto
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
            var logoutResponse = await _client.PostAsJsonAsync("/api/auth/logout", new RefreshTokenEntryDto
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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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
            var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

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

        [Fact]
        public async Task RequestResetAsync_WhenEmailIsValid_ReturnsOk()
        {
            // Arrange
            var request = new RequestPasswordResetDto
            {
                Email = "admin@admin.com" // Correo electrónico válido
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/request-reset", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RequestResetAsync_WhenEmailIsInvalid_ReturnsNotFound()
        {
            // Arrange
            var request = new RequestPasswordResetDto
            {
                Email = "invalid-email@example.com" // Correo electrónico inválido
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/request-reset", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            
        }
    }

    public class ResetPasswordTests : AuthControllerTests
    {
        public ResetPasswordTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ResetPassword_PasswordDoesNotMatch_ReturnsBadRequest()
        {
            //aranque
            var request = new ResetPasswordDto
            {
                Token = Models.PasswordRecoveryTokens.TestValidTokenAsynk.ResetToken,
                NewPassword = "1234565557",
                ConfirmPassword = "123458888"
            };
            //act
            var response = await _client.PostAsJsonAsync("/api/auth/reset-password", request);
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ResetPassword_WhenTokenIsValid_ReturnsOk()
        {
            // Arrange
            var request = new ResetPasswordDto
            {
                Token = Models.PasswordRecoveryTokens.TestValidTokenAsynk.ResetToken,// Token válido
                NewPassword = "newPassword123",
                ConfirmPassword = "newPassword123"
                
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/reset-password", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
           
        }

        [Fact]
        public async Task ResetPassword_WhenTokenIsInvalid_ReturnsNotFound()
        {
            // Arrange
            var request = new ResetPasswordDto
            {
                Token = "invalid-token", // Token inválido
                NewPassword = "newPassword123",
                ConfirmPassword = "newPassword123"
                
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/reset-password", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            
        }

        [Fact]
        public async Task ResetPassword_WhenTokenIsExpired_ReturnsBadRequest()
        {
            // Arrange
            var request = new ResetPasswordDto
            {
                Token = Models.PasswordRecoveryTokens.TestExpiredTokenAsynk.ResetToken, // Token expirado
                NewPassword = "newPassword123",
                ConfirmPassword = "newPassword123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/reset-password", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
        }
    }
}