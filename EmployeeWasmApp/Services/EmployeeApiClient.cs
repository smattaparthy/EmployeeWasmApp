
using System.Net.Http.Json;
using YourApp.Models;

namespace EmployeeWasmApp.Services;

public class EmployeeApiClient
{
    private readonly HttpClient _http;
    public EmployeeApiClient(HttpClient http) => _http = http;

    public Task<List<EmployeeInput>> GetAllAsync()
        => _http.GetFromJsonAsync<List<EmployeeInput>>("api/employees")!;

    public async Task<EmployeeInput> CreateAsync(EmployeeInput emp)
    {
        var resp = await _http.PostAsJsonAsync("api/employees", emp);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<EmployeeInput>())!;
    }
}
