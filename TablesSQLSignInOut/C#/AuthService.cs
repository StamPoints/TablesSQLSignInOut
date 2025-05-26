public class AuthService : IAuthService
{
    public bool IsLoggedIn { get; private set; } = false;
    public string? CurrentUser { get; private set; }

    public void SignIn(string username)
    {
        IsLoggedIn = true;
        CurrentUser = username;
    }

    public void SignOut()
    {
        IsLoggedIn = false;
        CurrentUser = null;
    }
}
