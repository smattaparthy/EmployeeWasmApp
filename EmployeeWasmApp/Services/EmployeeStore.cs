
using Microsoft.JSInterop;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using YourApp.Models;

namespace EmployeeWasmApp.Services;

public class EmployeeStore
{
    private const string Key = "employees";
    private readonly IJSRuntime _js;

    private readonly List<EmployeeInput> _employees = new();
    public IReadOnlyList<EmployeeInput> Employees => _employees;

    public EmployeeStore(IJSRuntime js) => _js = js;

    public async Task LoadAsync()
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", Key);

        _employees.Clear();

        if (!string.IsNullOrWhiteSpace(json))
        {
            var data = JsonSerializer.Deserialize<List<EmployeeInput>>(json);
            if (data is not null) _employees.AddRange(data);
        }
    }

    public EmployeeInput? GetById(Guid id)
        => _employees.FirstOrDefault(e => e.Id == id);

    public async Task AddAsync(EmployeeInput emp)
    {
        _employees.Add(new EmployeeInput
        {
            Id = Guid.NewGuid(),
            EmployeeId = emp.EmployeeId,
            LanId = emp.LanId,
            FirstName = emp.FirstName,
            MiddleInitial = emp.MiddleInitial,
            LastName = emp.LastName,
            EmailAddress = emp.EmailAddress,
            PhoneNumber = emp.PhoneNumber,
            Location = emp.Location,
            CostCenterNumber = emp.CostCenterNumber,
            StartDate = emp.StartDate,
            SeparationDate = emp.SeparationDate
        });

        await SaveAsync();
    }

    public async Task UpdateAsync(EmployeeInput emp)
    {
        var existing = GetById(emp.Id);
        if (existing is null) return;

        existing.EmployeeId = emp.EmployeeId;
        existing.LanId = emp.LanId;
        existing.FirstName = emp.FirstName;
        existing.MiddleInitial = emp.MiddleInitial;
        existing.LastName = emp.LastName;
        existing.EmailAddress = emp.EmailAddress;
        existing.PhoneNumber = emp.PhoneNumber;
        existing.Location = emp.Location;
        existing.CostCenterNumber = emp.CostCenterNumber;
        existing.StartDate = emp.StartDate;
        existing.SeparationDate = emp.SeparationDate;

        await SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = GetById(id);
        if (existing is null) return;

        _employees.Remove(existing);
        await SaveAsync();
    }

    public async Task ClearAsync()
    {
        _employees.Clear();
        await _js.InvokeVoidAsync("localStorage.removeItem", Key);
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_employees);
        await _js.InvokeVoidAsync("localStorage.setItem", Key, json);
    }
}
