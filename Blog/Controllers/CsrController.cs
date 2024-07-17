using Blog.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/csr")]
    public class CsrController : Controller
    {
        [HttpGet("GenerateCsr/{commonName?}")]
        public IActionResult GenerateCsr(string commonName)
        {
            if (string.IsNullOrWhiteSpace(commonName))
            {
                return BadRequest("Common Name is required");
            }

            string subjectName = $"CN={commonName}";
            string base64Csr;

            byte[] csr = CsrHelper.GenerateCsr(subjectName, out base64Csr);

            return Ok(new { csr = base64Csr });
        }
    }
}
