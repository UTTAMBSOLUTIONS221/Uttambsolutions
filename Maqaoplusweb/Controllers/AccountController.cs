using DBL;
using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Maqaoplusweb.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly BL bl;
        public AccountController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Addsystemstaffdata(SystemStaff model)
        {
            var resp = await bl.Registersystemstaffdata(model);
            return Json(resp);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Signin(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signin(Userloginmodel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var resp = await bl.ValidateSystemStaff(model.username, model.password);
            if (resp.RespStatus == 0)
            {
                if (resp.Usermodel.Loginstatus == (int)UserLoginStatus.VerifyAccount)
                {
                    return RedirectToAction("Verifyaccount", "Account", new { Code = Guid.NewGuid(), Usercode = Guid.NewGuid(), Staffid = resp.Usermodel.Userid, Accountnumber = resp.Usermodel.Accountnumber, Phonenumber = resp.Usermodel.Phonenumber });
                }
                else if (resp.Usermodel.Updateprofile)
                {
                    return RedirectToAction("Updatemyprofile", "Account", new { Code = Guid.NewGuid(), Usercode = Guid.NewGuid(), Staffid = resp.Usermodel.Userid, });
                }
                SetUserLoggedIn(resp, false);
                return RedirectToLocal(returnUrl);
            }
            else if (resp.RespStatus == 401)
            {
                Warning(resp.RespMessage, true);
            }
            else
            {
                ModelState.AddModelError(string.Empty, resp.RespMessage);
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Updatemyprofile(long Staffid)
        {
            var data = await bl.Getsystemstaffdatabyid(Staffid);
            return View(data);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Updatesystemstaffprofiledata(SystemStaff model)
        {
            var resp = await bl.Registersystemstaffdata(model);
            return Json(resp);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Verifyaccount(long Staffid, long Accountnumber, string Phonenumber)
        {
            Verifyaccountmodel data = new Verifyaccountmodel();
            data.Userid = Staffid;
            data.Accountnumber = Accountnumber;
            data.Phonenumber = Phonenumber;
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Myprofile()
        {
            var data = await bl.Getsystemuserprofiledata(SessionUserData.Usermodel.Userid);
            return View(data);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forgotpassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Forgotpassword(Forgotpassword model)
        {
            var resp = await bl.ValidateSystemForgotpasswordStaff(model);
            if (resp.Data != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error. Contact Admin");
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPermissions(long RoleId)
        {
            var permissions = await bl.Getsystempermissiondatabyroleid(RoleId);
            return Json(permissions);
        }
        public async Task<JsonResult> Getsystemstaffdatabyidnumber(int Idnumber)
        {
            var resp = await bl.Getsystemstaffdatabyidnumber(Idnumber);
            return Json(resp);
        }
        #region Other Methods

        private async void SetUserLoggedIn(UsermodelResponce user, bool rememberMe)
        {
            string userData = JsonConvert.SerializeObject(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Usermodel.Userid.ToString()),
                new Claim(ClaimTypes.Name, user.Usermodel.Fullname),
                new Claim("FullNames", user.Usermodel.Fullname),
                new Claim("Token", user.Token),
                new Claim("userData", userData),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");

            ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity[] { claimsIdentity });
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = new DateTimeOffset?(DateTime.UtcNow.AddMinutes(30))
            });
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Dashboard), "Home", new { area = "" });
            }
        }
        #endregion

    }
}
