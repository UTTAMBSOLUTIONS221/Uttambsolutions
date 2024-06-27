using DBL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private string connString;
        private bool _disposed;

        private IAccountRepository accountRepository;
        private IModulesRepository modulesRepository;
        private IBlogsRepository blogsRepository;


        public UnitOfWork(string connectionString) => connString = connectionString;
        public IAccountRepository AccountRepository
        {
            get { return accountRepository ?? (accountRepository = new AccountRepository(connString)); }
        }
        public IModulesRepository ModulesRepository
        {
            get { return modulesRepository ?? (modulesRepository = new ModulesRepository(connString)); }
        }
        public IBlogsRepository BlogsRepository
        {
            get { return blogsRepository ?? (blogsRepository = new BlogsRepository(connString)); }
        }

        public void Reset()
        {
            accountRepository = null;
            modulesRepository = null;
            blogsRepository = null;
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
