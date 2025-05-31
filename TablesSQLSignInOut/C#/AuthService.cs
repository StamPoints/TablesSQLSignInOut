using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly TestDataDbContext _context;
    private readonly CustomAuthenticationStateProvider _authStateProvider;

    public AuthService(TestDataDbContext context, CustomAuthenticationStateProvider authStateProvider)
    {
        _context = context;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        if (user != null && user.Password == password)
        {
            await _authStateProvider.MarkUserAsAuthenticated(user.UserName);
            return true;
        }
        return false;
    }

    public async Task LogoutAsync()
    {
        await _authStateProvider.MarkUserAsLoggedOut();
    }
}
