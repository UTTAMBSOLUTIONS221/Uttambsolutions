using DBL.Repositories;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        IGeneralRepository GeneralRepository { get; }
        IRoleRepository RoleRepository { get; }
        IAccountRepository AccountRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IModulesRepository ModulesRepository { get; }
        IBlogsRepository BlogsRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }

    }
}
