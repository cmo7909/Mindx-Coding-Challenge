using System.ComponentModel;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace CodeChallenge.services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository compRepo;
        private readonly IEmployeeRepository employeeRepo;

        public CompensationService(ICompensationRepository compensationRepo, IEmployeeRepository empRepo)
        {
            compRepo = compensationRepo;
            employeeRepo = empRepo;
        }

        // Returns the latest compensation for the employee or null
        public Compensation GetByEmployeeId(string employeeId)
        {
            return compRepo.GetByEmployeeId(employeeId);
        }

        // Creates a compensation record after ensuring the employee exists
        public Compensation Create(Compensation comp)
        {
            if (comp == null)
            {
                return null;
            }
            var emp = employeeRepo.GetById(comp.EmployeeId);
            if (emp == null)
            {
                return null;
            }
            comp.Employee = emp;
            return compRepo.Create(comp);
        }
    }
}