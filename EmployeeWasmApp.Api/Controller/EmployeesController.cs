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




dotnet ef migrations add InitialCreate `
  --project .\EmployeeWasmApp.Infrastructure\EmployeeWasmApp.Infrastructure.csproj `
  --startup-project .\EmployeeWasmApp.Api\EmployeeWasmApp.Api.csproj `
  --context EmployeeWasmApp.Infrastructure.Data.AppDbContext




dotnet ef database update `
  --project .\EmployeeWasmApp.Infrastructure\EmployeeWasmApp.Infrastructure.csproj `
  --startup-project .\EmployeeWasmApp.Api\EmployeeWasmApp.Api.csproj `
  --context EmployeeWasmApp.Infrastructure.Data.AppDbContext





using EmployeeWasmApp.Infrastructure.Entities;
using EmployeeWasmApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeWasmApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;

    public EmployeesController(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public Task<List<Employee>> GetAll(CancellationToken ct)
        => _repo.GetAllAsync(ct);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Employee>> GetById(Guid id, CancellationToken ct)
    {
        var emp = await _repo.GetByIdAsync(id, ct);
        return emp is null ? NotFound() : emp;
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create(Employee employee, CancellationToken ct)
    {
        var created = await _repo.AddAsync(employee, ct);
        return Ok(created);
    }
}
