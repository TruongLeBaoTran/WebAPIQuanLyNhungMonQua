using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPI_GiftManagement_BaoTran.Authorization;
using WebAPI_GiftManagement_BaoTran.Data;
using WebAPI_GiftManagement_BaoTran.Mappers;
using WebAPI_GiftManagement_BaoTran.Repository;
using WebAPI_GiftManagement_BaoTran.Services;
using WebAPI_GiftManagement_BaoTran.Validators;

namespace WebAPI_GiftManagement_BaoTran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            //Authentication - Đăng ký JWTBear
            string secretKey = builder.Configuration["AppSettings:SecretKey"];
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tự cấp token 
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ký vào token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero
                    };
                });


            //Đăng ký appsetting database
            IConfigurationRoot cf = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            builder.Services.AddDbContext<MyDbContext>(opt => opt.UseSqlServer(cf.GetConnectionString("MyDatabase")));

            //Đăng ký service
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IGiftService, GiftService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IPromotionService, PromotionService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
            builder.Services.AddScoped<IRoleUserService, RoleUserService>();
            builder.Services.AddScoped<IRankingService, RankingService>();


            //builder.Services.AddScoped<IValidator<Token>, AuthValidator>();
            //builder.Services.AddScoped<IValidator<GiftRequest>, GiftValidator>();
            //builder.Services.AddScoped<IValidator<UserRequest>, UserValidator>();
            //builder.Services.AddScoped<IValidator<RoleRequest>, RoleValidator>();
            //builder.Services.AddScoped<IValidator<PermissionRequest>, PermissionValidator>();
            //builder.Services.AddScoped<IValidator<RolePermissionRequest>, RolePermissionValidator>();
            //builder.Services.AddScoped<IValidator<PromoGiftRequest>, PromoGiftValidator>();

            //Đăng ký Repository
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>(); //Repository & unit of work

            //Đăng ký AutoMap
            builder.Services.AddAutoMapper(typeof(Mapping));

            //Đăng ký Fluent Validator
            builder.Services.AddControllers().AddFluentValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<AuthValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GiftValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RolePermissionValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RoleUserValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<PromotionValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RoleValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<PermissionValidator>();

            // Đăng ký JwtSecurityTokenHandler
            builder.Services.AddSingleton<JwtSecurityTokenHandler>();
            builder.Services.AddSingleton<TokenValidationParameters>(provider =>
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                byte[] key = Encoding.UTF8.GetBytes(configuration["AppSettings:SecretKey"]);
                return new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
            });

            // Đăng ký các dịch vụ liên quan đến phân quyền
            builder.Services.AddScoped<IUserPermissions, UserPermissions>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            //Cấu hình Hangfire
            builder.Services.AddHangfire(configuration =>
                    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("MyDatabase")));
            builder.Services.AddHangfireServer();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseAuthentication(); //khai báo

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
