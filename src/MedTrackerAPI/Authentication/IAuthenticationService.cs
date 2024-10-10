namespace MedTrackerAPI.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterUserAsync(string email, string password);
}