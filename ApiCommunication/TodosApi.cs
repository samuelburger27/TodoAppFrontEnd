using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace WebAss.ApiCommunication;

public class TodosApi
{
    private readonly HttpClient _httpClient;

    public TodosApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TodoItem>?> GetAllTodos()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "todoitems");
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        result.Sort(TodoItem.Compare);
        return result;
    }
    
    public async Task<TodoItem?> GetTodo(int id)
    {
        TodoItem? result = null;
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"todoitems/{id}");
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await _httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        var json = await response.Content.ReadAsStringAsync();
        result = JsonSerializer.Deserialize<TodoItem>(json);
        return result;
    }

    public async Task<bool> AddTodo(TodoItem todoItem)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "todoitems")
        {
            Content = jsonContent
        };
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await _httpClient.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }
    

    public async Task<bool> DeleteTodo(int id)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"todoitems/{id}");
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await _httpClient.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> EditTodo(TodoItem todoItem)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(todoItem), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"todoitems/{todoItem.id}")
        {
            Content = jsonContent
        };
        requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        var response = await _httpClient.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }
}