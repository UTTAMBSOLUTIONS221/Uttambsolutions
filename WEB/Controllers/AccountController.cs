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

namespace WEB.Controllers
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
        public async Task<JsonResult> Updatesystemstaffprofiledata(SystemStaff model)
        {
            var resp = await bl.Registersystemportalstaffdata(model);
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
                    return RedirectToAction("Verifyaccount", "Account", new { Code = Guid.NewGuid(), Usercode = Guid.NewGuid(), Staffid = resp.Usermodel.Userid, Phonenumber = resp.Usermodel.Phonenumber });
                }
                else if (resp.Usermodel.Updateprofile)
                {
                    return RedirectToAction("Updatemyprofile", "Account", new { Code = Guid.NewGuid(), Usercode = Guid.NewGuid(), Staffid = resp.Usermodel.Userid, });
                }
                SetUserLoggedIn(resp, false);
                return RedirectToLocal(returnUrl, resp.Usermodel.Rolename);
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
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Verifyaccount(long Staffid, string Phonenumber)
        {
            var data = await bl.Getsystemstaffdatabyid(Staffid);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Myprofile(string? Modulename = "job-listing-and-cv-revamping", int page = 0, int pageSize = 10)
        {
            SystemUserProfileData profileModel = new SystemUserProfileData();
            profileModel.Systemmodulename = Modulename;
            if (Modulename == "job-listing-and-cv-revamping")
            {
                profileModel.Systemjobsdata = await bl.Getsystemallopportunitydata(page, pageSize);
            }
            else if (Modulename == "latest-news-emerging-trends-and-politics")
            {
                profileModel.Systemblogdata = await bl.Getsystemallblogdata(page, pageSize);
            }
            else if (Modulename == "ecommerce-and-market-place")
            {
                profileModel.Shopproductsdata = await bl.Getsystemorganizationshopproductsdata();
            }
            //var data = await bl.Getsystemuserprofiledata(SessionUserData.Usermodel.Userid);
            return View(profileModel);
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
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPermissions(long RoleId)
        {
            var permissions = await bl.Getsystempermissiondatabyroleid(RoleId);
            return Json(permissions);
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
        private IActionResult RedirectToLocal(string returnUrl, string rolename)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (rolename == "Default User")
                {
                    return RedirectToAction(nameof(HomeController.Visitingmodule), "Home", new { area = "" });
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Dashboard), "Home", new { area = "" });
                }
            }
        }
        #endregion


        #region Profile Data via Ajax
        public async Task<JsonResult> GetJobs(int page, int pageSize)
        {
            var jobs = await bl.Getsystemallopportunitydata(page, pageSize);
            var totalPages = 1;

            var result = new
            {
                items = jobs.Systemjobs,
                pageIndex = page,
                totalPages = totalPages
            };

            return Json(result);
        }


        #endregion


        [HttpGet]
        public async Task<IActionResult> Blogdetails(long Blogid, string Pageid)
        {
            var blogPost = await bl.Getsystemblogdatabyid(Blogid);
            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Shopproductdetail(long Shopproductid)
        {
            var commerceProducts = await bl.Getsystemorganizationshopproductsdatabyid(Shopproductid);
            return View(commerceProducts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Jobdetails(long JobId)
        {

            // Retrieve job data based on the provided JobId
            var jobData = await bl.Getsystemopportunitydatabyid(JobId);
            return View(jobData);
        }
        [HttpGet]
        public async Task<IActionResult> Easyapplythisjob(long jobid)
        {
            var jobData = await bl.Getsystemopportunitydatabyid(jobid);
            var viewModel = new JobApplicationViewModel
            {
                Job = jobData,
                User = SessionUserData.Usermodel
            };
            var model = new SystemUserLog
            {
                Userid = SessionUserData.Usermodel.Userid,
                Logaction = "Viewing the easy apply page for job application",
                Browser = GetUserBrowser(),
                Ipaddress = Audit.GetIPAddress(),
                Loyaltyreward = 0,
                Loyaltystatus = 1,
                Logactionexittime = 0,
                Datecreated = DateTime.Now,
            };
            bl.Logsystemuseractivitydata(JsonConvert.SerializeObject(model));
            return View(viewModel);
        }

    }
}
