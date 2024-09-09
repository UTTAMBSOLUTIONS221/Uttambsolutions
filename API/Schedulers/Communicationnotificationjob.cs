﻿using DBL;
using Quartz;

namespace API.Schedulers
{
    public class Communicationnotificationjob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        public Communicationnotificationjob(IServiceProvider provider, IConfiguration config)
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
            var unsentEmailAddress = await bl.Getsystemunsentemaildata();
            if (unsentEmailAddress != null && unsentEmailAddress.Any())
            {
                foreach (var unsentEmailData in unsentEmailAddress)
                {
                    var Socialpages = await bl.Getsystemallsocialmediadata();
                    foreach (var page in Socialpages)
                    {
                        await bl.Updatepublishedblogdata(unsentEmailData.Blogid);
                    }
                }
                await Task.CompletedTask;
            }
        }

        public void Logs(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QuartzInvoices");
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
