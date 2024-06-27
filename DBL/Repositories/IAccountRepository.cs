using DBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public interface IAccountRepository
    {
        UsermodelResponce VerifySystemStaff(string Username);
    }
}
