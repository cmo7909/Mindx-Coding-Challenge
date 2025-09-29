using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;
using System.Runtime.CompilerServices;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        //Could not load direct reports without a new get by ID method, this method returns the reports to be used by my new methods
        //Altered after unit tests failed
        public Employee GetByIdWithReports(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var emp = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);

            if (emp == null) return null;

            if (emp.DirectReports == null || emp.DirectReports.Count == 0)
            {
                var children = _employeeContext.Employees
                    .Where(e => EF.Property<string>(e, "EmployeeId1") == id)
                    .ToList();

                if (children.Count > 0)
                    emp.DirectReports = children;
            }

            return emp;
        }
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
