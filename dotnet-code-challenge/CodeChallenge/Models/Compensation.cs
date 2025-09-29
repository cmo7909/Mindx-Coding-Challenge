using System;
using CodeChallenge.Models;

//Represents a compensation record for an employee
public class Compensation
{
    public int CompensationId { get; set; }

    public string EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public decimal Salary { get; set; }

    public DateTime EffectiveDate { get; set; }
}