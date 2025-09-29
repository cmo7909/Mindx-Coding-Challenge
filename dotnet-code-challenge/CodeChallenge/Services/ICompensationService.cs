using CodeChallenge.Models;

namespace CodeChallenge.services
{
    public interface ICompensationService
    {
        // Creates a new compensation record
        Compensation Create(Compensation comp);
        // Gets the most recent compensation for an employee by id
        Compensation GetByEmployeeId(string employeeId);
    }
}
