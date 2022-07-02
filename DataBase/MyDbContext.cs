using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDentityLearning.DataBase
{
    public class MyDbContext : IdentityDbContext<MyUser,MyRole,Guid>//有多种重载版本可以让DbContext继承
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {

        }


    } 
}
