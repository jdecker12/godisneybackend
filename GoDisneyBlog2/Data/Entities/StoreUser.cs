using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GoDisneyBlog2.Data.Entities
{
    public class StoreUser : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
    }
}
