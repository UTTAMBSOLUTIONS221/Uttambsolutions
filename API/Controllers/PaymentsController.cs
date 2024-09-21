using API.Paymentservices;
using DBL;
using DBL.Entities.PaymentEntities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly EquityJengaApiService _jengaApiService;

        public PaymentsController(EquityJengaApiService jengaApiService)
        {
            _jengaApiService = jengaApiService;
        }

        [HttpPost("authenticate/merchant")]
        public async Task<IActionResult> AuthenticateMerchant([FromBody] MerchantAuthenticationRequest requestData)
        {
            try
            {
                EquityJengaApi mpesaApi = new EquityJengaApi();

                var respons = await mpesaApi.GetMPesaAuthTokenAsync(requestData);
                //var response = await _jengaApiService.AuthenticateMerchantAsync(request);
                return Ok(respons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("verifyequity")]
        public async Task<IActionResult> VerifyEquityPayment(PaymentValidationRequest ValidationRequest)
        {
            try
            {
                EquityJengaApi mpesaApi = new EquityJengaApi();
                MerchantAuthenticationRequest requestData = new MerchantAuthenticationRequest
                {
                    MerchantCode = "9044190243",
                    ConsumerSecret = "ov86823y3dnWB9iRAFXN3ZMChni28z"
                };
                string TokenBearer = await mpesaApi.GetMPesaAuthTokenAsync(requestData);
                var verificationResponse = await _jengaApiService.VerifyPaymentAsync(TokenBearer, ValidationRequest);
                return Ok(verificationResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
