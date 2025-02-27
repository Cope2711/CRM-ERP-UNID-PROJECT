using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    
    /// <summary>
    /// Generates a new access token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the access token is generated.</param>
    /// <returns>A string representing the newly generated access token.</returns>
    Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId);
    
    /// <summary>
    /// Generates and stores a new refresh token for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the refresh token is generated.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the newly generated refresh token.
    /// </returns>
    Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken);
    
    /// <summary>
    /// Retrieves a refresh token by its token string.
    /// </summary>
    /// <param name="refreshToken">The refresh token string.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the refresh token if found; otherwise, null.
    /// </returns>
    Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken);
    
    /// <summary>
    /// Revokes a specific refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to revoke.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the revoked refresh token.
    /// </returns>
    Task RevokeRefreshsTokensByUserId(Guid userId);
    
    /// <summary>
    /// Revokes all refresh tokens associated with the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose refresh tokens are to be revoked.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
}