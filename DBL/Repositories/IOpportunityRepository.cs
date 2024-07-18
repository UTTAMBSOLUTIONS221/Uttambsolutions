using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IOpportunityRepository
    {
        Systemjobdata Getsystemallopportuntydata(int Page, int PageSize);
        Genericmodel Registersystemopportuntydata(string JsonData);
        SystemJob Getsystemopportuntydatabyid(long Opportunityid);
        IEnumerable<SystemJob> Getsystemallunpublishedopportunitydata();
        Genericmodel Updatepublishedopportunitydata(long Opportunityid);
    }
}
