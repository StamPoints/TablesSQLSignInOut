public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly CustomAuthenticationStateProvider _customAuthStateProvider;

    public AuthService(IHttpClientFactory httpClientFactory, CustomAuthenticationStateProvider customAuthStateProvider)
    {
        _httpClient = httpClientFactory.CreateClient();
        _customAuthStateProvider = customAuthStateProvider;
    }
    public async Task<bool> Login(string username, string password)
    {
        // Implement your login logic here, e.g., send a request to your API
        // and retrieve the JWT token upon successful authentication.

        var token = await RetrieveTokenFromApi(username, password);

        if (!string.IsNullOrEmpty(token))
        {
            await _customAuthStateProvider.MarkUserAsAuthenticated(token);
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _customAuthStateProvider.MarkUserAsLoggedOut();
    }

    private async Task<string> RetrieveTokenFromApi(string username, string password)
    {
        // Replace this with your actual API call to retrieve the token.
        // This is just a placeholder for demonstration purposes.
        return await Task.FromResult("your-jwt-token");
    }
}
