using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using Microsoft.AspNetCore.Mvc.Diagnostics;
#nullable enable //adding for null references

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if (employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }
        

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if (originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        //helper functiomn that recursivly counts the reports for an employee and its "children"
        private int CountReports(Employee employee)
        {
            if (employee == null || string.IsNullOrWhiteSpace(employee.EmployeeId))
                return 0;

            
            var current = _employeeRepository.GetByIdWithReports(employee.EmployeeId);
            if (current?.DirectReports == null || current.DirectReports.Count == 0)
                return 0;

            var total = 0;

            foreach (var emp in current.DirectReports)
            {
                if (emp == null || string.IsNullOrWhiteSpace(emp.EmployeeId))
                    continue;

                total += 1;            
                total += CountReports(emp); 
            }

            return total;
        }


        //Function that gathers employee ID and calls count reports to return a type with the necessary data (takes an employee ID string)
        public ReportingStructure? GetReportingStructure(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                return null;

            var employee = _employeeRepository.GetByIdWithReports(employeeId);

            if (employee == null)
                return null;

            var reports = CountReports(employee);

            return new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = reports
            };

        }

    }
}
