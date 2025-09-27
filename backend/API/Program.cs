
using API.Extentions;
using API.Middleware;
using API.Policy;
using Application;
using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Mappers;
using Application.Repositories;
using Application.Validators.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repositories;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //Cors frontEnd
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000") // FE origin
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // nếu bạn gửi cookie (refresh_token)
                    });
            });
            //kết nối DB
            builder.Services.AddInfrastructue(builder.Configuration.GetConnectionString("DefaultConnection")!);
            //Cache
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:Configuration"];
                options.InstanceName = builder.Configuration["Redis:InstanceName"];
            });
            //thêm httpcontextAccessor để lấy context trong service
            builder.Services.AddHttpContextAccessor();
            //Add scope repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IOTPRepository, OTPRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            //Add scope service
            builder.Services.AddScoped<IUserService, UserService>();

            //Fluentvalidator
            builder.Services.AddValidatorsFromAssemblyContaining(typeof(UserLoginReqValidator));
            builder.Services.AddFluentValidationAutoValidation();

            //Mapper
            //builder.Services.AddAutoMapper(typeof(UserProfile)); // auto mapper sẽ tự động scan hết assembly đó và xem tất cả thằng kết thừa Profile rồi tạo lun
                                                                 // mình chỉ cần truyền một thằng đại diện thoi

            //configure <-> setting
            //JWT
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var _jwtSetting = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddJwtTokenValidation(_jwtSetting!);
            //Email
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            //Otp
            builder.Services.Configure<OTPSettings>(builder.Configuration.GetSection("OTPSettings"));
            //middleware
            builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();







            //Cấu hình request nhận request, nó tự chuyển trường của các đối tượng trong
            //DTO thành snakeCase để binding giá trị, và lúc trả ra 
            //thì các trường trong respone cũng sẽ bị chỉnh thành snake case
            //Ảnh hưởng khi map từ json sang object và object về json : json <-> object
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });



            var app = builder.Build();
            //accept frontend
            app.UseCors("AllowFrontend");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
