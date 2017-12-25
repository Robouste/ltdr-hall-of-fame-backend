using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ltdr_hall_of_fame_backend.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HallOfFameContext(
                serviceProvider.GetRequiredService<DbContextOptions<HallOfFameContext>>()))
            {
                if (context.Jokes.Any())
                {
                    return;
                }

                var robouste = new User
                {
                    Id = 1,
                    Name = "Robouste",
                    Password = "d1c413827e6ba5fcf11761829f95a7d4",
                    Description = "Grand Maitre",
                    Role = "gourou"
                };
                var claire = new User
                {
                    Id = 2,
                    Name = "Fullerena",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "disciple"
                };
                var plasmap = new User
                {
                    Id = 3,
                    Name = "Plasmap",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "executeur"
                };
                var adrio = new User
                {
                    Id = 4,
                    Name = "Adrio",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "Une gène",
                    Role = "executeur"
                };
                var nargh = new User
                {
                    Id = 5,
                    Name = "Nargh",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "disciple"
                };
                var klakass = new User
                {
                    Id = 6,
                    Name = "Klakass",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "disciple"
                };
                var thaomas = new User
                {
                    Id = 7,
                    Name = "Thaomas",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "executeur"
                };
                var pyros = new User
                {
                    Id = 8,
                    Name = "Pywos",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "executeur"
                };
                var lemmin = new User
                {
                    Id = 9,
                    Name = "Lemmin",
                    Password = "098f6bcd4621d373cade4e832627b4f6",
                    Description = "",
                    Role = "disciple"
                };

                var joke1 = new Joke
                {
                    Description = "Perlinpinping",
                    Author = "Fullerena"
                };

                var joke2 = new Joke
                {
                    Description = "Tu trouves pas que ce mouchoir sent le chloroforme ?",
                    Author = "Robouste"
                };

                context.Jokes.AddRange(
                    joke1,
                    joke2
                );

                context.Votes.AddRange(
                    new Vote
                    {
                        User = robouste,
                        VoteState = 1,
                        Joke = joke2
                    },
                    new Vote
                    {
                        User = robouste,
                        VoteState = -1,
                        Joke = joke1
                    },
                    new Vote
                    {
                        User = claire,
                        VoteState = 1,
                        Joke = joke2
                    }
                );

                context.Users.AddRange(
                    robouste, claire, plasmap, adrio, klakass, nargh, pyros, thaomas, lemmin
                );

                context.SaveChanges();
            }
        }
    }
}
