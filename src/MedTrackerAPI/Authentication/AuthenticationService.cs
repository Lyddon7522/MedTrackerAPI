using FirebaseAdmin.Auth;

namespace MedTrackerAPI.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    public async Task<string> RegisterUserAsync(string email, string password)
    {
        var userArgs = new UserRecordArgs
        {
            Email = email,
            Password = password
        };
        
        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

        return userRecord.Uid;
    }
}