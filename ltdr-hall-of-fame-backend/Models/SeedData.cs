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
                    Name = "Robouste"
                };

                var claire = new User
                {
                    Id = 2,
                    Name = "Claire"
                };

                var joke1 = new Joke
                {
                    Description = "Perlinpinping",
                    Author = "Claire"
                };

                var joke2 = new Joke
                {
                    Description = "Random Joke",
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

                context.SaveChanges();
            }
        }
    }
}
