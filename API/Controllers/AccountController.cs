using DBL;
using DBL.Entities;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IWebHostEnvironment _env;

        public AccountController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
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
                     new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                     new Claim("UserId", _userData.Usermodel.Userid.ToString()),
                 };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kwq5R2Nmf4FWs03Hdx"));
            //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(
            //    _config["Jwt:Issuer"],
            //    _config["Jwt:Audience"],
            //    claims,
            //    expires: DateTime.UtcNow.AddMinutes(30),
            //    signingCredentials: signIn);
            //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var accessToken = GenerateJwtToken(_userData.Usermodel.Username, "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kwq5R2Nmf4FWs03Hdx", "JWTAuthenticationServer", "JWTServicePostmanClient");
            var refreshToken = GenerateRefreshTokenAsync(_userData.Usermodel.Userid);

            return Ok(new UsermodelResponce
            {
                RespStatus = 200,
                RespMessage = "Ok",
                Token = accessToken,
                Usermodel = _userData.Usermodel
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var Data = await bl.Getsystemstaffdatabyrefreshtoken(request.RefreshToken);
            if (Data == null || Data.Expirydate < DateTime.UtcNow)
            {
                return Unauthorized();
            }
            var newAccessToken = GenerateJwtToken(Data.Username, "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kwq5R2Nmf4FWs03Hdx", "JWTAuthenticationServer", "JWTServicePostmanClient");

            return Ok(new { accessToken = newAccessToken });
        }
        private async Task<string> GenerateRefreshTokenAsync(long userId)
        {
            var token = Guid.NewGuid().ToString();
            var expiryDate = DateTime.UtcNow.AddDays(7);
            RefreshToken refreshtoken = new RefreshToken
            {
                Token = token,
                Expirydate = expiryDate,
                Userid = userId
            };
            var responce = await bl.SaveStaffRefreshToken(JsonConvert.SerializeObject(refreshtoken));
            return token;
        }
        [AllowAnonymous]
        [Route("Registersystemuserdevicedata"), HttpPost]
        public async Task<Genericmodel> Registersystemuserdevicedata(DeviceInfoModel Model)
        {
            return await bl.Registersystemuserdevicedata(JsonConvert.SerializeObject(Model));
        }

        [AllowAnonymous]
        [Route("Registerstaff"), HttpPost]
        public async Task<Genericmodel> Registersystemstaffdata(SystemStaff Model)
        {
            return await bl.Registersystemstaffdata(Model);
        }

        [AllowAnonymous]
        [Route("Forgotstaffpassword"), HttpPost]
        public async Task<ForgotPasswordUserResponce> Forgotstaffpassword(Forgotpassword Model)
        {
            return await bl.ValidateSystemForgotpasswordStaff(Model);
        }

        [AllowAnonymous]
        [Route("Getsystemstaffprofiledatabyid/{UserId}"), HttpGet]
        public async Task<SystemStaffData> Getsystemstaffprofiledatabyid(long UserId)
        {
            return await bl.Getsystemstaffprofiledatabyid(UserId);
        }
        [AllowAnonymous]
        [Route("Getsystemstaffdetaildatabyid/{UserId}"), HttpGet]
        public async Task<Systemstaffdetaildata> Getsystemstaffdetaildatabyid(long UserId)
        {
            return await bl.Getsystemstaffdetaildatabyid(UserId);
        }
        [AllowAnonymous]
        [Route("Getsystemstaffdetaildatabyidnumber/{Idnumber}"), HttpGet]
        public async Task<Systemtenantdetailsdata> Getsystemstaffdetaildatabyidnumber(int Idnumber)
        {
            return await bl.Getsystemstaffdetaildatabyidnumber(Idnumber);
        }


        public static string GenerateJwtToken(string username, string secretKey, string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
