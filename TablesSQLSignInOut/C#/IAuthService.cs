public interface IAuthService
{
    bool IsLoggedIn { get; }
    string? CurrentUser { get; }
    void SignIn(string username);
    void SignOut();
}
