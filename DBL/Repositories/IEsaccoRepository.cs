using DBL.Models;

namespace DBL.Repositories
{
    public interface IEsaccoRepository
    {
        #region Esacco Sacco Administrator Summary
        Saccosummarydatamodel Getsaccosummarymodeldata(int Staffid);
        #endregion
    }
}
