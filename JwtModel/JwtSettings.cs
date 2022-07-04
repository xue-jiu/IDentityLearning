using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDentityLearning.JwtModel
{
    public class JwtSettings
    {
        public string ScrKey { get; set; }
        public int ExpireSecond { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
