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
        IEnumerable<SocialMediaSettings> Getsystemalllinkedinsocialmediadata();
        SocialMediaSettings Getsystemlinkedinsocialmediadata(string PageId);
        Genericmodel Updatelinkedinpagetoken(long SoicialId, string Appid, string AccessToken, string RefreshToken, int ExpiresIn);
        SocialMediaSettings Getsystemlinkedinsocialmediadatabyappid(string Appid);
        Genericmodel Updateaccesstokenonlinkedinpagetoken(long SoicialId, string Appid, string AccessToken);
    }
}
