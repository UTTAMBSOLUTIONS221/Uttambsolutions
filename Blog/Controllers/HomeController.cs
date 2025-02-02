using Blog.Models;
using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public HomeController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
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
        public async Task<IActionResult> Blogdetails(long Blogid, string Pageid)
        {
            var blogPost = await bl.Getsystemblogdatabyid(Blogid); // Fetch your blog post
            @ViewData["app_id"] = Pageid;
            @ViewData["type"] = "website";
            @ViewData["Title"] = blogPost.Blogname;
            @ViewData["description"] = blogPost.Summary;
            @ViewData["Blogownername"] = blogPost.Blogownername;
            @ViewData["image"] = blogPost.Blogprimaryimageurl;
            @ViewData["imagealt"] = blogPost.Blogcategoryname;
            @ViewData["Blogtags"] = blogPost.Blogtags;
            @ViewData["url"] = $"https://fortysevennews.uttambsolutions.com/Home/Blogdetails?code={Guid.NewGuid()}&Blogid={Blogid}";
            return View(blogPost);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
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
