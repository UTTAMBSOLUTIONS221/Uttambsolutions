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
        private ISocialmediaRepository socialmediaRepository;
        private IOrganizationRepository organizationRepository;
        private IModulesRepository modulesRepository;
        private ICronjobsRepository cronjobsRepository;
        private IBlogcategoryRepository blogcategoryRepository;
        private IBlogsRepository blogsRepository;
        private IOpportunityRepository opportunityRepository;
        private IBrandRepository brandRepository;
        private ICategoryRepository categoryRepository;
        private IProductRepository productRepository;
        private IPropertyRepository propertyRepository;


        private IServiceofferingRepository serviceofferingRepository;


        private IPaymentRepository paymentRepository;
        private IPesaServiceRepository pesaServiceRepository;
        private IParceldropRepository parceldropRepository;

        #region Esacco Sacco Administrator Summary
        private IEsaccoRepository esaccoRepository;
        #endregion
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
        public ISocialmediaRepository SocialmediaRepository
        {
            get { return socialmediaRepository ?? (socialmediaRepository = new SocialmediaRepository(connString)); }
        }
        public IOrganizationRepository OrganizationRepository
        {
            get { return organizationRepository ?? (organizationRepository = new OrganizationRepository(connString)); }
        }
        public IModulesRepository ModulesRepository
        {
            get { return modulesRepository ?? (modulesRepository = new ModulesRepository(connString)); }
        }
        public ICronjobsRepository CronjobsRepository
        {
            get { return cronjobsRepository ?? (cronjobsRepository = new CronjobsRepository(connString)); }
        }
        public IBlogcategoryRepository BlogcategoryRepository
        {
            get { return blogcategoryRepository ?? (blogcategoryRepository = new BlogcategoryRepository(connString)); }
        }
        public IBlogsRepository BlogsRepository
        {
            get { return blogsRepository ?? (blogsRepository = new BlogsRepository(connString)); }
        }
        public IOpportunityRepository OpportunityRepository
        {
            get { return opportunityRepository ?? (opportunityRepository = new OpportunityRepository(connString)); }
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
        public IPropertyRepository PropertyRepository
        {
            get { return propertyRepository ?? (propertyRepository = new PropertyRepository(connString)); }
        }


        public IServiceofferingRepository ServiceofferingRepository
        {
            get { return serviceofferingRepository ?? (serviceofferingRepository = new ServiceofferingRepository(connString)); }
        }



        public IPaymentRepository PaymentRepository
        {
            get { return paymentRepository ?? (paymentRepository = new PaymentRepository(connString)); }
        }
        public IPesaServiceRepository PesaServiceRepository
        {
            get { return pesaServiceRepository ?? (pesaServiceRepository = new PesaServiceRepository(connString)); }
        }
        public IParceldropRepository ParceldropRepository
        {
            get { return parceldropRepository ?? (parceldropRepository = new ParceldropRepository(connString)); }
        }
        #region Esacco Sacco Administrator Summary
        public IEsaccoRepository EsaccoRepository
        {
            get { return esaccoRepository ?? (esaccoRepository = new EsaccoRepository(connString)); }
        }
        #endregion
        public void Reset()
        {
            generalRepository = null;
            roleRepository = null;
            accountRepository = null;
            socialmediaRepository = null;
            modulesRepository = null;
            blogcategoryRepository = null;
            blogsRepository = null;
            opportunityRepository = null;
            brandRepository = null;
            categoryRepository = null;
            productRepository = null;
            propertyRepository = null;



            paymentRepository = null;
            pesaServiceRepository = null;
            parceldropRepository = null;

            esaccoRepository = null;

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
