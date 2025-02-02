﻿using DBL;
using DBL.Models.Mpesa;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace API.Controllers
{
    [Produces("application/json")]
    public class MPesaController : Controller
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public MPesaController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        #region MPESA C2B
        [HttpGet, Route("api/v1/channelm/expr/callback/{id:int}")]
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

        [HttpGet, Route("api/v1/channelm/c2b/validate/{id:int}")]
        public async Task<C2BValidationResp> Validation(int id)
        {
            try
            {
                //---- Read data
                string content = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                Util.LogError("C2B-Validation", new Exception(content), false);
                //await bl.ProcessMPesaSTKCallback(content);
            }
            catch (Exception ex)
            {
                Util.LogError("C2B-Validation", ex);
            }

            return new C2BValidationResp
            {
                ResultCode = 0,
                ResultDesc = "Success",
                ThirdPartyTransID = new Random().Next(100000, 999999).ToString()
            };
        }

        [HttpGet, Route("api/v1/channelm/c2b/confirm/{id:int}")]
        public async Task<C2BConfirmResp> Confirmation(int id)
        {
            try
            {
                //---- Read data
                string content = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                Util.LogError("C2B-Confirmation >> No:" + id, new Exception(content), false);
                bl.ProcessC2BConfirmation(id, content);
            }
            catch (Exception ex)
            {
                Util.LogError("C2B-Confirmation", ex);
            }

            return new C2BConfirmResp
            {
                ResultCode = 0,
                ResultDesc = "Success"
            };
        }
        #endregion

        #region MPESA B2C
        [HttpPost]
        [Route("api/v1/channelm/b2c/result")]
        public async Task<B2CResp> MPesaB2CResults()
        {
            try
            {
                //---- Read data
                string content = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                Util.LogError("MPesa-MPesaB2CResults Now", new Exception(content), false);
                //bl.ProcessB2CResult(id, content);
            }
            catch (Exception ex)
            {
                Util.LogError("MPesa-MPesaB2CResults", ex);
            }

            return new B2CResp();
        }
        [HttpPost]
        [Route("api/v1/channelm/b2c/timeout")]
        public async Task<B2CResp> MPesaB2CTimeout()
        {
            try
            {
                //---- Read data
                string content = "";
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    content = await reader.ReadToEndAsync();
                }

                Util.LogError("Pesa-MPesaB2CTimeout Now", new Exception(content), false);
                //bl.ProcessB2CResult(id, content);
            }
            catch (Exception ex)
            {
                Util.LogError("MPesa-MPesaB2CTimeout", ex);
            }

            return new B2CResp();
        }
        #endregion
    }
}
