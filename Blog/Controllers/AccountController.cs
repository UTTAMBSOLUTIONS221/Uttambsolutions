﻿using Blog.Entities;
using DBL;
using DBL.Entities;
using DBL.Enum;
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
            if (ModelState.IsValid)
            {
                var resp = await bl.ValidateSystemForgotpasswordStaff(model.Emailaddress);
                if (resp.RespStatus == 0)
                {
                    Success(resp.RespMessage, true);
                    return RedirectToAction("Index", "Home");
                }
                else if (resp.RespStatus == 1)
                {
                    Warning(resp.RespMessage, true);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, resp.RespMessage);
                }
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

        #endregion
    }
}