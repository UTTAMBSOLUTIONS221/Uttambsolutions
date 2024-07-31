using DBL.Repositories;

namespace DBL.UOW
{
    public interface IUnitOfWork
    {
        IGeneralRepository GeneralRepository { get; }
        IRoleRepository RoleRepository { get; }
        IAccountRepository AccountRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        ISocialmediaRepository SocialmediaRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IModulesRepository ModulesRepository { get; }

        IBlogcategoryRepository BlogcategoryRepository { get; }

        IBlogsRepository BlogsRepository { get; }
        IOpportunityRepository OpportunityRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IPropertyRepository PropertyRepository { get; }

    }
}
