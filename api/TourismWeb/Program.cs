using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TourismWeb.Data;
using TourismWeb.Mappings;
using TourismWeb.Repositories.Implementations;
using TourismWeb.Repositories.Interfaces;
using TourismWeb.Services.Implementations;
using TourismWeb.Services.Interfaces;
using TourismWeb.Helpers;
using TourismWeb.Models;
using NLog.Web; 
using NLog; 

namespace TourismWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Khởi tạo logger cho ứng dụng
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting application...");

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    });

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp",
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:3000")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                });

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                EmailSendingHelper.Instance.SetConfiguration(builder.Configuration);
                builder.Services.AddDbContext<TourismDbContext>(options => options.UseSqlServer(connectionString));
                builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
                builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<IAccountService, AccountService>();
                builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
                builder.Services.AddScoped<ICustomerService, CustomerService>();
                builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();
                builder.Services.AddScoped<IImageRepository, ImageRepository>();
                builder.Services.AddScoped<IImageService, ImageService>();
                builder.Services.AddScoped<ITourRepository, TourRepository>();
                builder.Services.AddScoped<ITourService, TourService>();
                builder.Services.AddScoped<ILocationRepository, LocationRepository>();
                builder.Services.AddScoped<ILocationService, LocationService>();
                builder.Services.AddScoped<IBookingRepository, BookingRepository>();
                builder.Services.AddScoped<IBookingService, BookingService>();
                builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
                builder.Services.AddScoped<IPaymentService, PaymentService>();
                builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
                builder.Services.AddScoped<IScheduleService, ScheduleService>();
                builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
                builder.Services.AddScoped<IReviewService, ReviewService>();
                builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

                var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
                var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
                var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                        };
                    });

                // Tích hợp NLog với ASP.NET Core
                builder.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders(); // Xóa các nhà cung cấp log mặc định
                    loggingBuilder.AddNLog("nlog.config"); // Sử dụng NLog
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("AllowReactApp");
                app.UseStaticFiles();

                app.UseAuthentication(); // Thêm authentication trước authorization
                app.UseAuthorization();

                app.MapControllers();

                logger.Info("Application is running...");
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped due to an error");
                throw;
            }
            finally
            {
                // Đóng NLog khi ứng dụng dừng
                LogManager.Shutdown();
            }
        }
    }
}