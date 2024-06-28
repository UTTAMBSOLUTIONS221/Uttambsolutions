using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        IGeneralRepository GeneralRepository { get; }
        IAccountRepository AccountRepository { get; }
        IModulesRepository ModulesRepository { get; }
        IBlogsRepository BlogsRepository { get; }
        ICategoryRepository CategoryRepository { get; }
    }
}
