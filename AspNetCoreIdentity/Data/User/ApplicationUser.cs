using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Data.User
{
    public class ApplicationUser : IdentityUser
    {
        public string Position { get; set; }
        public string Department { get; set; } 
    }
}
