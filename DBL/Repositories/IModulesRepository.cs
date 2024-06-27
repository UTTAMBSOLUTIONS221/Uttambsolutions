using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IModulesRepository
    {
        IEnumerable<Systemmodule> Getsystemmoduledata();
        Genericmodel Registersystemmoduledata(string JsonData);
        Systemmodule Getsystemmoduledatabyid(long Moduleid);
    }
}
