using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface ISocialmediaRepository
    {
        IEnumerable<SocialMediaSettings> Getsysteusersocialmediadata(long Userid);
        Genericmodel Registersystemsocialmediapagedata(string JsonData);
    }
}
