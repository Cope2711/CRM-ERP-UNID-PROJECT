using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

namespace Tests;

public class AuthControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ReturnsTokenDto()
    {
        // Arrange
        LoginUserDto loginUserDto = new LoginUserDto
        {
            UserUserName = "admin",
            UserPassword = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
        tokenDto.Should().NotBeNull();
        tokenDto.Token.Should().NotBeNullOrEmpty();
        tokenDto.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WhenUserNameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        LoginUserDto loginUserDto = new LoginUserDto
        {
            UserUserName = "non-existent-user",
            UserPassword = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Login_WhenPasswordIsInvalid_ReturnsUnauthorized()
    {
        // Arrange
        LoginUserDto loginUserDto = new LoginUserDto
        {
            UserUserName = "admin",
            UserPassword = "invalid-password"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WhenRefreshTokenIsValid_ReturnsTokenDto()
    {
        // Arrange
        LoginUserDto loginUserDto = new LoginUserDto
        {
            UserUserName = "admin",
            UserPassword = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginUserDto);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
        tokenDto.Should().NotBeNull();
        tokenDto.Token.Should().NotBeNullOrEmpty();
        tokenDto.RefreshToken.Should().NotBeNullOrEmpty();

        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = tokenDto.RefreshToken
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var refreshTokenDto = await refreshTokenResponse.Content.ReadFromJsonAsync<TokenDto>();
        refreshTokenDto.Should().NotBeNull();
        refreshTokenDto.Token.Should().NotBeNullOrEmpty();
        refreshTokenDto.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task RefreshToken_WhenRefreshTokenIsInvalid_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "invalid-refresh-token"
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WhenRefreshTokenIsExpired_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs="
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WhenRefreshTokenIsRevoked_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "+9wpQEQ3YJsBXCzLbutUMyIwGo1RenCAh7iKSCQEugg="
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WhenRefreshTokenIsValid_ReturnsTokenDto()
    {
        // Logg in
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginUserDto
        {
            UserUserName = Models.Users.TestUser.UserUserName,
            UserPassword = "123456"
        });
        
        loginResponse.Should().NotBeNull();
        loginResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        TokenDto? tokenDto = await loginResponse.Content.ReadFromJsonAsync<TokenDto>();

        tokenDto.Should().NotBeNull();
        tokenDto.Token.Should().NotBeNullOrEmpty();
        tokenDto.RefreshToken.Should().NotBeNullOrEmpty();

        // Logout
        var logoutResponse = await _client.PostAsJsonAsync("/api/auth/logout", new RefreshTokenEntryDto
        {
            RefreshToken = tokenDto.RefreshToken
        });

        logoutResponse.Should().NotBeNull();
        logoutResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        TokenDto? logoutTokenDto = await logoutResponse.Content.ReadFromJsonAsync<TokenDto>();

        logoutTokenDto.Should().NotBeNull();
        logoutTokenDto.Token.Should().BeNull();
        logoutTokenDto.RefreshToken.Should().NotBeNullOrEmpty();

    }

    [Fact]
    public async Task Logout_WhenRefreshTokenIsInvalid_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "invalid-refresh-token"
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WhenRefreshTokenIsExpired_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs="
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WhenRefreshTokenIsRevoked_ReturnsUnauthorized()
    {
        // Arrange
        RefreshTokenEntryDto refreshTokenEntryDto = new RefreshTokenEntryDto
        {
            RefreshToken = "+9wpQEQ3YJsBXCzLbutUMyIwGo1RenCAh7iKSCQEugg="
        };

        // Act
        var refreshTokenResponse = await _client.PostAsJsonAsync("/api/auth/logout", refreshTokenEntryDto);

        // Assert
        refreshTokenResponse.Should().NotBeNull();
        refreshTokenResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}