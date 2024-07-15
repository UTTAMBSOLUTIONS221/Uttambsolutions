using Blog.Models;
using DBL;
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
            var blogPost = await bl.Getsystemblogdatabyid(Blogid); // Fetch your blog post
            ViewBag.Title = blogPost.Blogname;
            ViewBag.OgTitle = blogPost.Blogname;
            ViewBag.OgDescription = blogPost.Summary;
            ViewBag.OgImage = blogPost.Blogprimaryimageurl;
            ViewBag.OgUrl = $"https://fortysevennews.uttambsolutions.com/Home/Blogdetails?code={Guid.NewGuid()}&Blogid={Blogid}";
            ;
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
