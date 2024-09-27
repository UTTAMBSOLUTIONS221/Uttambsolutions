using DBL;
using Quartz;

namespace API.Schedulers
{
    public class Generatemonthlyrentinvoicejob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public Generatemonthlyrentinvoicejob(IServiceProvider provider, IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _provider = provider;
            _env = env;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd");
            var generateInvoices = await bl.Generatemonthlyrentinvoicedata();
            await Task.CompletedTask;
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
