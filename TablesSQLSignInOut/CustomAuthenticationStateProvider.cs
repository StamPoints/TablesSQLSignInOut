using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Retrieve authentication state from a secure source (e.g., session, cookie)
        return Task.FromResult(new AuthenticationState(_anonymous));
    }

    public Task MarkUserAsAuthenticated(string userName)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "apiauth");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        return Task.CompletedTask;
    }

    public Task MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        return Task.CompletedTask;
    }
}
