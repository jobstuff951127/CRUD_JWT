using System;
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
            if (!context.Costumers.Any())
            {
                var costumers = new List<Costumer>
                {
                    new Costumer
                    {
                        FirstName = "Memo",
                        LastName = "Herdez",
                        Cellphone = "352458448",
                        Adress = "Esta #2344",
                        BirthDate = DateTime.Now.Date.AddMonths(-2).ToShortDateString()
                    },
                    new Costumer
                    {
                        FirstName = "Nick",
                        LastName = "Gerz",
                        Cellphone = "551323221",
                        Adress = "White brow #244",
                        BirthDate = DateTime.Now.Date.AddMonths(-3).ToShortDateString(),
                        Status = true
                    },
                    new Costumer
                    {
                        FirstName = "Larry",
                        LastName = "Atota",
                        Cellphone = "952417881",
                        Adress = "Canionga #144",
                        BirthDate = DateTime.Now.Date.AddMonths(-4).ToShortDateString()
                    }
                };
                context.Costumers.AddRange(costumers);
                context.SaveChanges();
            }

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