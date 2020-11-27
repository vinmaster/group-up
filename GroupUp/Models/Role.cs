using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupUp.Models
{
    public class Role : IdentityRole<Guid>
    {
        public Role() { }
        public Role(string role)
        {
            Name = role;
        }
    }
}
