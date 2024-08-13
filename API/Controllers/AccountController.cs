using DBL;
using DBL.Entities;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BL bl;
        IConfiguration _config;
        public AccountController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
        }
        [AllowAnonymous]
        [Route("Authenticate"), HttpPost]
        public async Task<ActionResult> AuthenticateAsync([FromBody] Userloginmodel userdata)
        {
            var _userData = await bl.ValidateSystemStaff(userdata.username, userdata.password);
            if (_userData.RespStatus == 1)
                return Unauthorized(new UsermodelResponce
                {
                    RespStatus = 401,
                    RespMessage = _userData.RespMessage,
                    Token = "",
                    Usermodel = new UsermodeldataResponce()
                });
            if (_userData.RespStatus == 2)
                return StatusCode(StatusCodes.Status500InternalServerError, _userData.RespMessage);
            var claims = new[] {
                     new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                     new Claim("UserId", _userData.Usermodel.Userid.ToString()),
                 };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signIn);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new UsermodelResponce
            {
                RespStatus = 200,
                RespMessage = "Ok",
                Token = tokenString,
                Usermodel = _userData.Usermodel
            });
        }
        [AllowAnonymous]
        [Route("Registerstaff"), HttpPost]
        public async Task<Genericmodel> Registersystemstaffdata(SystemStaff Model)
        {
            return await bl.Registersystemstaffdata(Model);
        }

        [AllowAnonymous]
        [Route("Forgotstaffpassword"), HttpPost]
        public async Task<Genericmodel> Forgotstaffpassword(Forgotpassword Model)
        {
            return await bl.ValidateSystemForgotpasswordStaff(Model.Emailaddress);
        }

        [AllowAnonymous]
        [Route("Getsystemstaffdetaildatabyid/{UserId}"), HttpGet]
        public async Task<Systemdataffdata> Getsystemstaffdetaildatabyid(long UserId)
        {
            return await bl.Getsystemstaffdetaildatabyid(UserId);
        }
        [AllowAnonymous]
        [Route("Getsystemstaffdetaildatabyidnumber/{Idnumber}"), HttpGet]
        public async Task<Systemtenantdetailsdata> Getsystemstaffdetaildatabyidnumber(int Idnumber)
        {
            return await bl.Getsystemstaffdetaildatabyidnumber(Idnumber);
        }
    }
}
