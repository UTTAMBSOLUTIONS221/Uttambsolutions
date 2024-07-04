using Faithlink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faithlink.Services
{
    public interface IGroupManagementService
    {
        Task<List<GroupModel>> LoadGroupsAsync();
        Task JoinGroupAsync(GroupModel group, long userId);
    }
}
