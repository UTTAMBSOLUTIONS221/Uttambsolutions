using DBL;
using DBL.Helpers;
using Quartz;

namespace Jobs.Schedulers
{
    public class PublishJobandBlogtoLinkedinJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        LinkedinHelpers facebook = new LinkedinHelpers();
        public PublishJobandBlogtoLinkedinJob(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            bl = new BL(Util.ShareConnectionString(config));
        }
    }
}
