using DBL.Repositories;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        IGeneralRepository GeneralRepository { get; }
        IAccountRepository AccountRepository { get; }
        IModulesRepository ModulesRepository { get; }
        IBlogsRepository BlogsRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
    }
}
