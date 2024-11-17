using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoaaa.DAL.JwtHelper
{
    public class JWTModel
    {
        public String Key { get; set; }
        public String Issuer { get; set; }
        public String Audiance { get; set; }
        public double DurationInDays { get; set; }

    }
}
