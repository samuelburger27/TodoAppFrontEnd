using System.Net;
using System.Net.Http.Json;

namespace WebAss;

public class ApiController
{
    private const string Url = "http://localhost:5202/todoitems";
    private readonly HttpClient client = new(new HttpClientHandler
    {
        UseCookies = true, // This allows the browser to handle cookies
        Credentials = new NetworkCredential() // Ensure credentials are passed
    });

    public async Task<bool> LogIn(LoginSchema login)
    {
        var response = await client.PostAsJsonAsync("http://localhost:5202/login?useCookies=true&useSessionCookies=true", login);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<TodoItem>?> GetAllTodos()
    {
        
        return await client.GetFromJsonAsync<List<TodoItem>>(Url);
    }

    public async Task<TodoItem?> GetTodo(int id)
    {
        var targetUrl = $"{Url}/{id}";
        return await client.GetFromJsonAsync<TodoItem>(targetUrl);
    }

    public async Task<bool> AddTodo(TodoItem todoItem)
    {
        var response = await client.PostAsJsonAsync(Url, todoItem);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTodo(int id)
    {
        var targetUrl = $"{Url}/{id}";
        var response = await client.DeleteAsync(targetUrl);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EditTodo(TodoItem todoItem)
    {
        var targetUrl = $"{Url}/{todoItem.id}";
        var response = await client.PutAsJsonAsync(targetUrl, todoItem);
        return response.IsSuccessStatusCode;
    }
    
}