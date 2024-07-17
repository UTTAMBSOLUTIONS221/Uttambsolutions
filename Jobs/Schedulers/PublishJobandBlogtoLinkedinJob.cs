using DBL;
using DBL.Helpers;
using Quartz;

namespace Jobs.Schedulers
{
    public class PublishJobandBlogtoLinkedinJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        LinkedinHelpers linkedinHelper = new LinkedinHelpers();
        public PublishJobandBlogtoLinkedinJob(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            bl = new BL(Util.ShareConnectionString(config));
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
                //foreach (var blogData in unpublishedBlogs)
                //{
                //    //Get all Registered Social Pages
                //    var Socialpages = await bl.Getsystemallsocialmediadata();
                //    var accessToken = await linkedinHelper.GetAccessTokenAsync(clientId, clientSecret, redirectUri, authCode);

                //    var jobPosts = ExtractJobPostsFromBlog(blogUrl); // Implement this method to extract job posts from your blog

                //    foreach (var jobPost in jobPosts)
                //    {
                //        await linkedinHelper.PostJobToLinkedInAsync(accessToken, jobPost, companyPageId);
                //    }
                //}
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