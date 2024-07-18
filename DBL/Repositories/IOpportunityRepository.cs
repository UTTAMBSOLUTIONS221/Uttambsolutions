using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IOpportunityRepository
    {
        Systemjobdata Getsystemallopportunitydata(int Page, int PageSize);
        Genericmodel Registersystemopportunitydata(string JsonData);
        SystemJob Getsystemopportunitydatabyid(long Opportunityid);
        IEnumerable<SystemJob> Getsystemallunpublishedopportunitydata();
        Genericmodel Updatepublishedopportunitydata(long Opportunityid);
    }
}
