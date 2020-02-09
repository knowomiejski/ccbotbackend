using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Persistance
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Test User 1",
                        UserName = "test1",
                        Email = "test1@user.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Test User 2",
                        UserName = "test2",
                        Email = "test2@user.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Test User 3",
                        UserName = "test3",
                        Email = "test3@user.com"
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Test12345!");
                }
            }
            
            if (context.Settings.Any()) return;
            var settings = new List<Settings>
            {
                new Settings
                {
                    Name = "StandardSettings",
                    TargetChannel = "potatomaster4000",
                    Prefix = "!",
                    ReminderTimer = 120,
                    FolderId = "1byWobqcHkOQMBELGSXK1WL3Gjrp-_J5_"
                }
            };

            context.Settings.AddRange(settings);
            context.SaveChanges();
        }
    }
}