using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace WebAss.ApiCommunication;

public class CookieAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly HttpClient httpClient;

    public CookieAuthenticationStateProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "manage/info");
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        try
        {
            var userResponse = await httpClient.SendAsync(requestMessage);
            if (userResponse.IsSuccessStatusCode)
            {
                var userJson = await userResponse.Content.ReadAsStringAsync();
                var jsonSOptions = new JsonSerializerOptions
                    { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };
                var userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSOptions);
                if (userInfo != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, userInfo.Email!),
                        new(ClaimTypes.Email, userInfo.Email!)
                    };
                    var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
                    user = new ClaimsPrincipal(id);
                }
            }

            return new AuthenticationState(user);
        }
        catch
        { return new AuthenticationState(user); }
    }

    public async Task<bool> Login(LoginModel model)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "login?useCookies=true")
        {
            Content = jsonContent
        };
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var userResponse = await httpClient.SendAsync(requestMessage);
        
        if (!userResponse.IsSuccessStatusCode) return false;
        
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

    public async Task<bool> Register(LoginModel model)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "register")
        {
            Content = jsonContent
        };
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var userResponse = await httpClient.SendAsync(requestMessage);
        return userResponse.IsSuccessStatusCode;
    }

    public async Task<bool> LogOut()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "logout");
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await httpClient.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> IsLoggedIn()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity.IsAuthenticated;
    }
}