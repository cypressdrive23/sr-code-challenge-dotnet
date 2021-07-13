using challenge.Models;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation Create(Compensation compensation);
        Compensation GetById(string id);
    }
}