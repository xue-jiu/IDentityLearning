using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IDentityLearning.DataBase
{
    public class MyUser:IdentityUser<Guid>
    {
        public string MyPreference { get; set; }
    }
}
