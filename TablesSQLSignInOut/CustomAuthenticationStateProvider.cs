using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Replace with your logic to retrieve the authenticated user
        var user = GetUserFromSessionOrDatabase();

        if (user != null)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("WorkID", user.WorkID.ToString())
            }, "Custom authentication");

            var principal = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(principal));
        }

        return Task.FromResult(new AuthenticationState(_anonymous));
    }

    // Implement this method to retrieve the user from session or database
    private User GetUserFromSessionOrDatabase()
    {
        // Your logic here
        return null;
    }
    public Task MarkUserAsAuthenticated(User user)
    {
        var identity = new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("WorkID", user.WorkID.ToString())
    }, "Custom authentication");

        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        return Task.CompletedTask;
    }

}
