﻿using DBL;
using DBL.Helpers;
using Quartz;

namespace Blog.Schedulers
{
    public class PublishBlogstoFacebookJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        FacebookHelper facebook = new FacebookHelper();
        private readonly IWebHostEnvironment _env;

        public PublishBlogstoFacebookJob(IServiceProvider provider, IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd");
            //get all unpublished blogs and not published
            var unpublishedBlogs = await bl.Getsystemallunpublishedblogdata();
            if (unpublishedBlogs != null && unpublishedBlogs.Any())
            {
                foreach (var blogData in unpublishedBlogs)
                {
                    //Get all Registered Social Pages
                    var Socialpages = await bl.Getsystemallsocialmediadata();
                    foreach (var page in Socialpages)
                    {
                        var imageUrls = new List<string>
                        {
                            blogData.Blogprimaryimageurl
                        };
                        string blogUrl = $"https://fortysevennews.uttambsolutions.com/Home/Blogdetails?code={Guid.NewGuid()}&Blogid={blogData.Blogid}";
                        //string resp = await facebook.PublishBlogPostAsync(page.PageAccessToken, page.UserAccessToken, page.PageId, blogData.Summary, blogUrl);
                        await bl.Updatepublishedblogdata(blogData.Blogid);
                    }
                }
                await Task.CompletedTask;
            }
        }
        public void Logs(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Quartz");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "Logs.txt");
            using FileStream fstream = new FileStream(path, FileMode.Append);
            using TextWriter writer = new StreamWriter(fstream);
            writer.WriteLine(message);
        }
    }
}
