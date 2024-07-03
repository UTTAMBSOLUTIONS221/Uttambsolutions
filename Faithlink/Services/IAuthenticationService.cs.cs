using Faithlink.Models;

namespace Faithlink.Services
{
    public interface IAuthenticationService
    {
        Task<UsermodelResponce> Validateuser(string email, string password);
    }
}
