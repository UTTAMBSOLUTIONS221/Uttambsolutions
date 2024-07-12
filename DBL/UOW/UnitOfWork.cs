using DBL.Repositories;

namespace DBL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private string connString;
        private bool _disposed;

        private IGeneralRepository generalRepository;
        private IRoleRepository roleRepository;
        private IAccountRepository accountRepository;
        private ISettingsRepository settingsRepository;
        private IOrganizationRepository organizationRepository;
        private IModulesRepository modulesRepository;
        private IBlogcategoryRepository blogcategoryRepository;
        private IBlogsRepository blogsRepository;
        private IBrandRepository brandRepository;
        private ICategoryRepository categoryRepository;
        private IProductRepository productRepository;



        public UnitOfWork(string connectionString) => connString = connectionString;
        public IGeneralRepository GeneralRepository
        {
            get { return generalRepository ?? (generalRepository = new GeneralRepository(connString)); }
        }
        public IRoleRepository RoleRepository
        {
            get { return roleRepository ?? (roleRepository = new RoleRepository(connString)); }
        }
        public IAccountRepository AccountRepository
        {
            get { return accountRepository ?? (accountRepository = new AccountRepository(connString)); }
        }
        public ISettingsRepository SettingsRepository
        {
            get { return settingsRepository ?? (settingsRepository = new SettingsRepository(connString)); }
        }
        public IOrganizationRepository OrganizationRepository
        {
            get { return organizationRepository ?? (organizationRepository = new OrganizationRepository(connString)); }
        }
        public IModulesRepository ModulesRepository
        {
            get { return modulesRepository ?? (modulesRepository = new ModulesRepository(connString)); }
        }
        public IBlogcategoryRepository BlogcategoryRepository
        {
            get { return blogcategoryRepository ?? (blogcategoryRepository = new BlogcategoryRepository(connString)); }
        }
        public IBlogsRepository BlogsRepository
        {
            get { return blogsRepository ?? (blogsRepository = new BlogsRepository(connString)); }
        }
        public ICategoryRepository CategoryRepository
        {
            get { return categoryRepository ?? (categoryRepository = new CategoryRepository(connString)); }
        }
        public IBrandRepository BrandRepository
        {
            get { return brandRepository ?? (brandRepository = new BrandRepository(connString)); }
        }
        public IProductRepository ProductRepository
        {
            get { return productRepository ?? (productRepository = new ProductRepository(connString)); }
        }

        public void Reset()
        {
            generalRepository = null;
            roleRepository = null;
            accountRepository = null;
            modulesRepository = null;
            blogcategoryRepository = null;
            blogsRepository = null;
            brandRepository = null;
            categoryRepository = null;
            productRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
