using DBL;
using DBL.Entities;
using DBL.Enum;
using DBL.Helpers;
using DBL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Blog.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly FacebookHelper _facebookHelper;
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public AccountController(FacebookHelper facebookHelper, IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
            _facebookHelper = facebookHelper;
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
        public async Task<IActionResult> Register(SystemStaff model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var resp = await bl.Registersystemstaffdata(model);
                    if (resp.RespStatus == 0)
                    {
                        Success(resp.RespMessage, true);
                        return RedirectToAction("Signin", "Account");
                    }
                    else if (resp.RespStatus == 401)
                    {
                        Warning(resp.RespMessage, true);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, resp.RespMessage);
                    }
                }
                catch (Exception ex)
                {
                    Util.LogError("Register Staff", ex, true);
                }
            }
            return View();
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
                SetUserLoggedIn(resp, false);

                //if (resp.LoginStatus == (int)UserLoginStatus.VerifyAccount)
                //{
                //    return RedirectToAction("VerifyAccount", "Account", new { Usercode = resp.Userid, Phonenumber = resp.Phone });
                //}
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Myprofile()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            var data = await bl.Getsystemuserprofiledata(SessionUserData.Usermodel.Userid);
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
        public IActionResult Forgotpassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Forgotpassword(Forgotpassword model)
        {
            if (ModelState.IsValid)
            {
                var resp = await bl.ValidateSystemForgotpasswordStaff(model);
                //if (resp.RespStatus == 0)
                //{
                //    Success(resp.RespMessage, true);
                //    return RedirectToAction("Index", "Home");
                //}
                //else if (resp.RespStatus == 1)
                //{
                //    Warning(resp.RespMessage, true);
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, resp.RespMessage);
                //}
            }
            return View();
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
                return RedirectToAction(nameof(AccountController.Myprofile), "Account", new { area = "" });
            }
        }
        #endregion

        #region Other Methods

        #region Add Personal Blogs
        [HttpGet]
        public async Task<IActionResult> Addsystemblog(int Blogid)
        {
            ViewData["Systemblogcategorylists"] = bl.GetListModel(ListModelType.SystemBlogCategory).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Systemblog blogData = new Systemblog();
            if (Blogid > 0)
            {
                blogData = await bl.Getsystemblogdatabyid(Blogid);
            }
            return PartialView(blogData);
        }
        public async Task<JsonResult> Addsystemblogdata(Systemblog model)
        {
            var resp = await bl.Registersystemblogdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        #endregion

        #region Add Social Pages
        [HttpGet]
        public async Task<IActionResult> Addsocialmediapage(long Socialsettingid)
        {
            SocialMediaSettings socialMediaSettings = new SocialMediaSettings();
            if (Socialsettingid > 0)
            {
                socialMediaSettings = await bl.Getsystemsocialmediadatabyid(Socialsettingid);
            }
            return PartialView(socialMediaSettings);
        }

        //public async Task<JsonResult> Addsocialmediapagedata(SocialMediaSettings model)
        //{
        //    Genericmodel Resp = new Genericmodel();
        //    var longLivedToken = await _facebookHelper.ExchangeAccessTokenAsync(model.Appid, model.Appsecret, model.UserAccessToken);
        //    var pageAccessTokenResponse = await _facebookHelper.GenerateNeverExpiresAccessTokenAsync(longLivedToken.AccessToken);

        //    var matchingPage = pageAccessTokenResponse.Data.FirstOrDefault(x => x.Name.Contains(model.Socialpagename, StringComparison.OrdinalIgnoreCase));
        //    if (matchingPage != null)
        //    {
        //        // Set the page access token and page ID
        //        model.PageAccessToken = matchingPage.AccessToken;
        //        model.PageId = matchingPage.Id;

        //        // Save the data
        //        Resp = await bl.Registersystemsocialmediapagedata(JsonConvert.SerializeObject(model));
        //    }
        //    else
        //    {
        //        // If the page name doesn't exist, return with an error message
        //        Resp.RespStatus = 1;
        //        Resp.RespMessage = "Failed to find the page with the specified name. Use correct facebook Page name";
        //    }

        //    return Json(Resp);
        //}
        #endregion

        #endregion
    }
}