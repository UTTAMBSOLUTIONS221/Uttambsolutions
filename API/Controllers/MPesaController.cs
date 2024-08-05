using DBL;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Controllers
{
    [Produces("application/json")]
    public class MPesaController : Controller
    {
        private readonly BL bl;
        public MPesaController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }

        #region MPESA C2B
        [HttpPost]
        [Route("api/v1/channelm/expr/callback/{id:int}")]
        public async Task MPesaSTKPushCallback(int id)
        {
            try
            {
                //---- Read data
                string content = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                Util.LogError("Pesa-STKPushCallback", new Exception(content), false);
                bl.ProcessMPesaSTKCallback(id, content);
            }
            catch (Exception ex)
            {
                Util.LogError("Pesa-MPesaSTKPushCallback", ex);
            }
        }

        #endregion
    }
}
