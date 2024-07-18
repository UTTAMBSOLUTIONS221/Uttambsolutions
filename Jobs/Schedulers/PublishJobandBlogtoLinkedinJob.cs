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
            var unpublishedopportunities = await bl.Getsystemallunpublishedopportunitydata();
            if (unpublishedopportunities != null && unpublishedopportunities.Any())
            {
                foreach (var opportunityData in unpublishedopportunities)
                {
                    string JobPostUrl = "";
                    opportunityData.JobPostUrl = JobPostUrl;
                    //Get all Registered Social Pages
                    var Socialpages = await bl.Getsystemalllinkedinsocialmediadata();
                    foreach (var social in Socialpages.Where(x => x.PageType == "Linkedin"))
                    {
                        var accessToken = "";
                        // var accessToken = await linkedinHelper.GetAccessTokenAsync(social.clientId, clientSecret, redirectUri, authCode);
                        await linkedinHelper.PostJobToLinkedInAsync(accessToken, opportunityData, social.PageId);
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