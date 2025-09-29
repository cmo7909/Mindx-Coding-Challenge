using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;
using System.Runtime.CompilerServices;

namespace CodeChallenge.Repositories;

public class CompensationRepositories : ICompensationRepository
{
    private readonly EmployeeContext context;

    public CompensationRepositories(EmployeeContext contx) => context = contx;

    // Add the entity and save immediately
    public Compensation Create(Compensation comp)
    {
        context.Compensations.Add(comp);
        context.SaveChanges();
        return comp;
    }

    // controller to return the full employee object in the JSON response
    public Compensation GetByEmployeeId(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
        {
            return null;
        }
        return context.Compensations
            .Include(c => c.Employee)
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.EffectiveDate)
            .FirstOrDefault();
    }
}