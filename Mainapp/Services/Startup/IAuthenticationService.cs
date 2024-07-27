using Mainapp.Models.Startup;

namespace Mainapp.Services.Startup
{
    public interface IAuthenticationService
    {
        Task<UsermodelResponce> Validateuser(string email, string password);
    }
}
