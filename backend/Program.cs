using backend.Models;
using backend.Repositories.EventRepository;
using backend.Repositories.EventStaffRepository;
using backend.Repositories.NewsRepository;
using backend.Repositories.PaymentRepository;
using backend.Repositories.TicketRepository;
using backend.Repositories.StaffRepository;
using backend.Repositories.StatisticRepository;
using backend.Repositories.UserRepository;
using backend.Repositories.EventRatingRepository;
using backend.Repositories.ForumRepository;
using backend.Services.EventService;
using backend.Services.EventStaffService;
using backend.Services.NewsService;
using backend.Services.PaymentService;
using backend.Services.TicketService;
using backend.Services.OtherService;
using backend.Services.StaffService;
using backend.Services.StatisticService;
using backend.Services.UserService;
using backend.Services.ForumService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;
using backend.Helper;
using DinkToPdf.Contracts;
using DinkToPdf;
using backend.Services.EventRatingService;
using backend.Repositories.PostCommentRepository;
using backend.Services.PostCommentService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5000); // HTTP
//    serverOptions.ListenAnyIP(5001, listenOptions =>
//    {
//        listenOptions.UseHttps(); // HTTPS
//    });
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
        {
            throw new ArgumentNullException("jwt config values are missing.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<FpttickethubContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FTHSystem")));
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton(new PayOS("b2514e3c-d2d6-432a-b1a4-eaaa8b989c88", "f8879890-fd24-41db-bc96-aa21ec3f7abd", "100f66bc876b8ed977fa4cde1864a4065394dc0e82e7f8a8b37a8e74d07da637"));
builder.Services.AddHostedService<EmailReminderService>();
builder.Services.AddHostedService<EmailRatingService>();
builder.Services.AddHostedService<StaffRemoveService>();
builder.Services.AddLogging();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var services = builder.Services;
services.AddHttpContextAccessor();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IEventRepository, EventRepository>();
services.AddScoped<IEventService, EventService>();
services.AddScoped<IEventStaffRepository, EventStaffRepository>();
services.AddScoped<IEventStaffService, EventStaffService>();
services.AddScoped<INewsRepository, NewsRepository>();
services.AddScoped<INewsService, NewsService>();
services.AddScoped<IPaymentRepository, PaymentRepository>();
services.AddScoped<IEventRatingRepository, EventRatingRepository>();
services.AddScoped<IPaymentService, PaymentService>();
services.AddScoped<ITicketRepository, TicketRepository>();
services.AddScoped<ITicketService, TicketService>();
services.AddScoped<IStaffRepository, StaffRepository>();
services.AddScoped<IStaffService, StaffService>();
services.AddScoped<IStatisticRepository, StatisticRepository>();
services.AddScoped<IStatisticService, StatisticService>();
services.AddScoped<IForumRepository, ForumRepository>();
services.AddScoped<IForumService, ForumService>();
services.AddScoped<IEventRatingService, EventRatingService>();
services.AddScoped<IPostCommentRepository, PostCommentRepository>();
services.AddScoped<IPostCommentService, PostCommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use CORS
app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();