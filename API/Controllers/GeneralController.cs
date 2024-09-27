using DBL;
using DBL.Enum;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GeneralController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;

        private readonly IWebHostEnvironment _env;

        public GeneralController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        [AllowAnonymous]
        public List<ListModel> Systemdropdowns(ListModelType listType)
        {
            return bl.GetListModel(listType).Result.Select(x => new ListModel
            {
                Text = x.Text,
                Value = x.Value,
                GroupId = x.GroupId,
                GroupName = x.GroupName,
            }).ToList();
        }
        [HttpGet("Getdropdownitembycode")]
        [AllowAnonymous]
        public List<ListModel> Systemdropdowns(ListModelType listType, long code)
        {
            return bl.GetListModelById(listType, code).Result.Select(x => new ListModel
            {
                Text = x.Text,
                Value = x.Value,
                GroupId = x.GroupId,
                GroupName = x.GroupName,
            }).ToList();
        }
    }
}
