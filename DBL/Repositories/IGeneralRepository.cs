using DBL.Enum;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IGeneralRepository
    {
        IEnumerable<ListModel> GetListModel(ListModelType listType);
        IEnumerable<ListModel> GetListModelbycode(ListModelType listType, long code);
    }
}
