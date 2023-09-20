using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.Data
{
    public class SeedDB
    {
        private static readonly List<ApplicationUser> normalUsers = new List<ApplicationUser>()
        {
            new ApplicationUser { Name = "Tariqul Alam", DateOfBirth = Convert.ToDateTime(DateTime.ParseExact("07-11-1997", "dd-MM-yyyy", CultureInfo.InvariantCulture)), UserName = "tariqul", PhoneNumber = "01683105317", IsRemoved = false, Email = "tariqul@bytequantic.com", LockoutEnabled = false },
            new ApplicationUser { Name = "Anwar Hossain", DateOfBirth = Convert.ToDateTime(DateTime.ParseExact("07-11-1981", "dd-MM-yyyy", CultureInfo.InvariantCulture)), UserName = "anwar", PhoneNumber = "01683105317", IsRemoved = false, Email = "anwar@bytequantic.com", LockoutEnabled = false }
        };

        private static readonly List<ApplicationUser> adminUsers = new List<ApplicationUser>()
        {
            new ApplicationUser { Name = "Admin", DateOfBirth = Convert.ToDateTime(DateTime.ParseExact("07-11-1997", "dd-MM-yyyy", CultureInfo.InvariantCulture)), UserName = "admin", PhoneNumber = "01683105317", IsRemoved = false, Email = "admin@bytequantic.com", LockoutEnabled = false },
            new ApplicationUser { Name = "Motiur Rahman", DateOfBirth = Convert.ToDateTime(DateTime.ParseExact("07-11-1981", "dd-MM-yyyy", CultureInfo.InvariantCulture)), UserName = "armaan", PhoneNumber = "01683105317", IsRemoved = false, Email = "armaan@bytequantic.com", LockoutEnabled = false }
        };

        private static readonly string[] adminRole = new string[] { "Admin" };
        private static readonly string[] userRole = new string[] { "Provider", "Rider", "Customer" };

        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            foreach (var role in userRole)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = role
                    });
                }
            }

            foreach (var role in adminRole)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = role
                    });
                }
            }

            //foreach (var user in normalUsers)
            //{
            //    if (await userManager.FindByNameAsync(user.UserName) == null)
            //    {
            //        var result = await userManager.CreateAsync(user);
            //        if (result.Succeeded)
            //        {
            //            await userManager.AddPasswordAsync(user, "Asd123@");
            //            await userManager.AddToRoleAsync(user, "User");
            //        }
            //    }
            //}

            foreach (var user in adminUsers)
            {
                if (await userManager.FindByNameAsync(user.UserName) == null)
                {
                    var result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await userManager.AddPasswordAsync(user, "Asd123@");
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
