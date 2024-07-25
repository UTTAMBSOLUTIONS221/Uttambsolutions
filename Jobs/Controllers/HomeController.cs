using DBL;
using DBL.Entities;
using DBL.Models;
using Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Jobs.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        private readonly BL bl;

        public HomeController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var jobsData = await bl.Getsystemallopportunitydata(0, 1000);
            return View(jobsData);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Jobdetails(long JobId)
        {

            // Retrieve job data based on the provided JobId
            var jobData = await bl.Getsystemopportunitydatabyid(JobId);
            // Set the image URL, using a default image if Employerlogo is null
            var imageUrl = string.IsNullOrEmpty(jobData.Organizationlogo)
                           ? "https://jobcenter.uttambsolutions.com/Images/uttambsolutionsjoblogo.png"
                           : jobData.Organizationlogo;

            // Populate ViewData with relevant information
            ViewData["image"] = imageUrl;
            ViewData["type"] = "website";
            ViewData["Title"] = jobData.Title;
            ViewData["description"] = jobData.JobDescription;
            ViewData["Blogownername"] = jobData.Employername;
            ViewData["imagealt"] = jobData.Functionname;
            ViewData["Blogtags"] = "JobCenter, job listings, freelancing, job biddings, career opportunities, .NET jobs, Python jobs, article writing, SQL Server, MSSQL, technical writing, recruitment agency, hiring, job search, employment, job board, job portal, IT jobs, software development, engineering jobs, healthcare jobs, finance jobs, marketing jobs, remote jobs, freelance jobs, contract jobs, part-time jobs, full-time jobs, internship, entry-level jobs, executive jobs, job applications, resume writing, career advice, interview tips, job fairs, talent acquisition, staffing solutions, headhunting, workforce solutions, career development, job training, job placement, skill development, professional networking, job alerts, recruitment services, job consultancy, employment agency, recruitment solutions, job opportunities, career hub, job finder, work opportunities, job postings, job openings, hiring now, employment opportunities, job vacancies, find a job, job seekers, talent pool, career growth, job support, online jobs, local jobs, international jobs, job market, job recommendations, industry-specific jobs, job matching, job board software, HR solutions, job outreach, talent management, employee recruitment, workforce management";
            ViewData["url"] = $"https://jobcenter.uttambsolutions.com/Home/Jobdetails?code={Guid.NewGuid()}&jobcode={Guid.NewGuid()}&JobId={JobId}";

            return View(jobData);
        }
        [HttpGet]
        public async Task<IActionResult> Easyapplythisjob(long jobid)
        {
            var jobData = await bl.Getsystemopportunitydatabyid(jobid);
            // Set the image URL, using a default image if Employerlogo is null
            // Set the image URL, using a default image if Employerlogo is null
            var imageUrl = string.IsNullOrEmpty(jobData.Organizationlogo)
                           ? "https://jobcenter.uttambsolutions.com/Images/uttambsolutionsjoblogo.png"
                           : jobData.Organizationlogo;

            // Populate ViewData with relevant information
            ViewData["image"] = imageUrl;
            ViewData["type"] = "website";
            ViewData["Title"] = jobData.Title;
            ViewData["description"] = jobData.JobDescription;
            ViewData["Blogownername"] = jobData.Employername;
            ViewData["imagealt"] = jobData.Functionname;
            ViewData["Blogtags"] = "JobCenter, job listings, freelancing, job biddings, career opportunities, .NET jobs, Python jobs, article writing, SQL Server, MSSQL, technical writing, recruitment agency, hiring, job search, employment, job board, job portal, IT jobs, software development, engineering jobs, healthcare jobs, finance jobs, marketing jobs, remote jobs, freelance jobs, contract jobs, part-time jobs, full-time jobs, internship, entry-level jobs, executive jobs, job applications, resume writing, career advice, interview tips, job fairs, talent acquisition, staffing solutions, headhunting, workforce solutions, career development, job training, job placement, skill development, professional networking, job alerts, recruitment services, job consultancy, employment agency, recruitment solutions, job opportunities, career hub, job finder, work opportunities, job postings, job openings, hiring now, employment opportunities, job vacancies, find a job, job seekers, talent pool, career growth, job support, online jobs, local jobs, international jobs, job market, job recommendations, industry-specific jobs, job matching, job board software, HR solutions, job outreach, talent management, employee recruitment, workforce management";
            ViewData["url"] = $"https://jobcenter.uttambsolutions.com/Home/Jobdetails?code={Guid.NewGuid()}&jobcode={Guid.NewGuid()}&JobId={jobid}";

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
