using IDentityLearning.DataBase;
using IDentityLearning.JwtModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDentityLearning
{
    public class Startup
    {
        public IConfiguration _confirguration { get; }
        public Startup(IConfiguration configuration)
        {
            _confirguration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Dbcontext注入
            services.AddDbContext<MyDbContext>(opt =>
            {
                opt.UseSqlServer(_confirguration["DbContext:ConnectionString"]);//得到的是string
            });
            services.AddDataProtection();
            //Identity
            //用于配置标识服务的 Helper 函数  盲猜IdentityBuilder调用了services中的函数/盲猜 IdentityBuilder时Service中的一个属性,或者Identity时Service中的一个属性,用Identity来生成
            //var IdentityBuilder = new IdentityBuilder(typeof(MyUser), typeof(MyRole), services);
            services.AddIdentityCore<MyUser>(opt => //与AddIdentity不同,AddIdentity还会添加默认界面
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                //密码生成规则
                opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;//密码重置时,生成较为简单的密码
                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddRoles<MyRole>()
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders()
            //.AddRoleManager<RoleManager<MyRole>>()//AddRoles包含了RoleManager
            .AddUserManager<UserManager<MyUser>>();


            //Jwt
            services.AddOptions().Configure<JwtSettings>(_confirguration.GetSection("Jwt"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
               //var JwtSettings=_confirguration.GetSection("Jwt").Get<JwtSettings>();
                //_confirguration["Jwt:SecretKey"]就可以用JwtSettings.SecretKey代替
                var secretByte = Encoding.UTF8.GetBytes(_confirguration["Jwt:ScrKey"]);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _confirguration["Jwt:Issuer"],//也可以写成_confirguration
                    ValidateAudience = true,
                    ValidAudience = _confirguration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
