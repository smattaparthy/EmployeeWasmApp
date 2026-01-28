using System;
using System.ComponentModel.DataAnnotations;

namespace YourApp.Models;

public class EmployeeInput
{
    [Required, StringLength(20)]
    public string EmployeeId { get; set; } = "";

    [StringLength(20)]
    public string? LanId { get; set; }

    [Required, StringLength(50)]
    public string FirstName { get; set; } = "";

    [StringLength(50)]
    public string? MiddleInitial { get; set; }

    [Required, StringLength(50)]
    public string LastName { get; set; } = "";

    [Required, EmailAddress]
    public string EmailAddress { get; set; } = "";

    [Phone]
    public string? PhoneNumber { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    [StringLength(20)]
    public string? CostCenterNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? SeparationDate { get; set; }
    public Guid Id { get; set; }
}
