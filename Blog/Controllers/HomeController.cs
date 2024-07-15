using Blog.Models;
using DBL;
using DBL.Models;
using DBL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly BL bl;
        private readonly FacebookService _facebookService;
        public HomeController(IConfiguration config, FacebookService facebookService)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _facebookService = facebookService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var blogposts = await bl.Getsystemallblogdata(0, 10000);
            return View(blogposts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogdetails(long Blogid)
        {
            string appId = "1049608423544508";
            string appSecret = "ea9bb3edec697f274262523d80840fe0";
            string accesstoken = "EAAO6nQE9FrwBO92BkZBNVkXgwZBVfRiZC02G8GU5gUBte0EWIssZCSqSsZAAEaEswD13FkfiR0dc3ycYczjyZB4piZC5fx5CZBzBBKX63riCDQVVCyekgoZBoPfbbcYIMPtmaZC3FO7YIhUx57SpwwwbSJZCyZC42VvmrCxUKmQbkIGT9RBhZA28F7iWgPlVpsHmWfFUD19G5mqobKZCZAI8TET94UxobfRFZAkZD";
            //get access token
            //FacebookAccessTokenResponse accessToken = await _facebookService.GetAccessTokenAsync(appId, appSecret);
            FacebookExchangeTokenResponse longlivedaccessToken = await _facebookService.ExchangeAccessTokenAsync(appId, appSecret, accesstoken);

            if (longlivedaccessToken.access_token != null)
            {
                //change access toke to longlived access token
                if (longlivedaccessToken.access_token != null)
                {
                    FacebookNeverExpiresResponse neverexpiresaccessToken = await _facebookService.Generatenevereexpiresaccesstoken(longlivedaccessToken.access_token);



                }
                else
                {
                    // Handle the case where the access token is null.
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to generate Facebook long lived access token.");
                }

            }
            else
            {
                // Handle the case where the access token is null.
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve Facebook access token.");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
