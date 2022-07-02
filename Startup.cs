using IDentityLearning.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddDbContext<MyDbContext>(opt=> 
            {
                opt.UseSqlServer(_confirguration["DbContext:ConnectionString"]);
            });
            services.AddDataProtection();
            //用于配置标识服务的 Helper 函数   盲猜 IdentityBuilder时Service中的一个属性,或者Identity时Service中的一个属性,用Identity来生成
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
            //.AddRoleManager<RoleManager<MyRole>>()
            .AddUserManager<UserManager<MyUser>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
