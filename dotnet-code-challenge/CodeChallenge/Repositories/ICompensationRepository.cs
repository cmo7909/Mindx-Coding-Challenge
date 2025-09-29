using CodeChallenge.Models;
namespace CodeChallenge.Repositories;

public interface ICompensationRepository
{
    //persists a new compensation record
    Compensation Create(Compensation comp);
    //Returns the most recent compensation for a given employeeId
    Compensation GetByEmployeeId(string EmployeeId);
}