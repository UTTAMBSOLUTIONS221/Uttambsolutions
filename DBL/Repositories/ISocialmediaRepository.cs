using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface ISocialmediaRepository
    {
        IEnumerable<SocialMediaSettings> Getsystemsocialmediadata(long Userid);
        Genericmodel Registersystemsocialmediapagedata(string JsonData);
        SocialMediaSettings Getsystemsocialmediadatabyid(long Socialsettingid);
        IEnumerable<SocialMediaSettings> Getsystemallsocialmediadata();
    }
}
