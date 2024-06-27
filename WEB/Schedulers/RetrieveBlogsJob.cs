﻿using System;
using System.IO;
using System.Threading.Tasks;
using DBL;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;
using Quartz;

namespace WEB.Schedulers
{
    public class RetrieveBlogsJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly BL bl;

        public RetrieveBlogsJob(IServiceProvider provider, IConfiguration config)
        {
            _provider = provider;
            bl = new BL(Util.ShareConnectionString(config));
        }

        public Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);
            string formattedDate = yesterday.ToString("yyyy-MM-dd");

            var newsApiClient = new NewsApiClient("ba5196a31b684d1194b4d161ad7dd5c6");
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = "Apple",
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                From = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day),
            });

            if (articlesResponse.Status == Statuses.Ok && articlesResponse.Articles != null)
            {
                var newsResponse = new DBL.Models.NewsResponse
                {
                    Status = articlesResponse.Status.ToString(),
                    TotalResults = articlesResponse.TotalResults,
                    Articles = articlesResponse.Articles.Select(article => new DBL.Models.Article
                    {
                        Source = new DBL.Models.Source
                        {
                            Id = article.Source.Id,
                            Name = article.Source.Name
                        },
                        Author = article.Author,
                        Title = article.Title,
                        Description = article.Description,
                        Url = article.Url,
                        UrlToImage = article.UrlToImage,
                        PublishedAt = article.PublishedAt ?? DateTime.MinValue,
                        Content = article.Content
                    }).ToList()
                };

                bl.RetrieveandSaveBlogs(JsonConvert.SerializeObject(newsResponse));
            }

            return Task.CompletedTask;
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
