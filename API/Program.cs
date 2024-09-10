using API.Schedulers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Quartz services
builder.Services.AddHostedService<QuartzHostedService>();
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Add our job
builder.Services.AddSingleton<Generatemonthlyrentinvoicejob>();
builder.Services.AddSingleton<Communicationnotificationjob>();
builder.Services.AddSingleton(new JobSchedule(
jobType: typeof(Generatemonthlyrentinvoicejob),
   cronExpression: "0 * * * * ?"));

//0 0 0 L * ?
builder.Services.AddSingleton(new JobSchedule(
    jobType: typeof(Communicationnotificationjob),
    cronExpression: "0 0 0 * * ?"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("http://localhost:3000", "http://localhost:3000") // Specify allowed origins
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

// Use CORS
app.UseCors("AllowSpecificOrigins");

app.UseAuthentication(); //Authentication
app.UseAuthorization(); //Authorization

app.MapControllers();
app.Run();