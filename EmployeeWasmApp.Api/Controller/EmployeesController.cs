using Microsoft.AspNetCore.Mvc;

namespace EmployeeWasmApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    // In-memory storage (temporary)
    private static readonly List<Employee> Employees = new();

    [HttpGet]
    public ActionResult<List<Employee>> GetAll()
        => Employees;

    [HttpGet("{id:guid}")]
    public ActionResult<Employee> GetById(Guid id)
    {
        var emp = Employees.FirstOrDefault(e => e.Id == id);
        return emp is null ? NotFound() : emp;
    }

    [HttpPost]
    public ActionResult<Employee> Create(Employee emp)
    {
        emp.Id = Guid.NewGuid();
        Employees.Add(emp);
        return CreatedAtAction(nameof(GetById), new { id = emp.Id }, emp);
    }
}

// Simple model inside the same file (for now)
public class Employee
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; } = "";
    public string? LanId { get; set; }
    public string FirstName { get; set; } = "";
    public string? MiddleInitial { get; set; }
    public string LastName { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
    public string? CostCenterNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? SeparationDate { get; set; }
}
namespace EmployeeWasmApp.Infrastructure.Integrations;

public interface IEmployeeDirectoryClient
{
    Task<string?> LookupEmailAsync(string lanId, CancellationToken ct = default);
}




namespace EmployeeWasmApp.Infrastructure.Integrations;

public class EmployeeDirectoryClient : IEmployeeDirectoryClient
{
    private readonly HttpClient _http;

    public EmployeeDirectoryClient(HttpClient http) => _http = http;

    public async Task<string?> LookupEmailAsync(string lanId, CancellationToken ct = default)
    {
        // Example: GET /directory/email/{lanId}
        var resp = await _http.GetAsync($"/directory/email/{lanId}", ct);
        if (!resp.IsSuccessStatusCode) return null;

        return await resp.Content.ReadAsStringAsync(ct);
    }
}

