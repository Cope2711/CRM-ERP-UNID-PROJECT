namespace CRM_ERP_UNID.Helpers;

public class HasherHelper
{
    public static string HashString(string item)
    {
        return BCrypt.Net.BCrypt.HashPassword(item);
    }

    public static bool VerifyHash(string hash, string item)
    {
        return BCrypt.Net.BCrypt.Verify(hash, item);
    }

    public static bool VerifyDeviceId(string hashedDeviceId, string deviceId)
    {
        string deviceIdHash = HashDeviceIdForStorage(deviceId);
        return hashedDeviceId == deviceIdHash;
    }
    
    public static string HashDeviceIdForStorage(string deviceId)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(deviceId);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

}