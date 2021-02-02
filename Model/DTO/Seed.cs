using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Model.DTO
{
    public class Seed
    {
        public static async Task SeedData(TokaContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        UserName = "Bob",
                        Email = "bob@test.com"
                    },
                    
                    new AppUser
                    {
                        UserName = "Memo",
                        Email = "memo@test.com"
                    },
                    new AppUser
                    {
                        UserName = "Eve",
                        Email = "eve@test.com"
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            } 

        }
    }
}