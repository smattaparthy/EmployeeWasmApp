namespace EmployeeWasmApp.Api.Models;

public class EmployeeDto
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
