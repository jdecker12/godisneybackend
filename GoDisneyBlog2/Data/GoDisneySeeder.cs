using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using GoDisneyBlog2.Data.Entities;
using GoDisneyBlog2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoDisneyBlog2.Data
{
    public class GoDisneySeeder
    {
        private readonly GoDisneyContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public GoDisneySeeder(GoDisneyContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("jeremydecker1@me.com");
            if (user == null)
            {


                user = new StoreUser()
                {
                    FName = "Jeremy",
                    LName = "Decker",
                    Email = "jeremydecker1@me.com",
                    UserName = "jdecker71"
                };

               

                var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder.");
                }
                _ctx.SaveChanges();
            }

            if(_ctx.Cards.Any())
            {
                var card = _ctx.Cards
                       .Where(c => c.Id == 4)
                       .FirstOrDefault();

                card.User = user;
            }

            _ctx.SaveChanges();
        }
    }
}

