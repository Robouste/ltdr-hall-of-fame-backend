using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ltdr_hall_of_fame_backend.Models
{
    public class HallOfFameContext: DbContext
    {
        public HallOfFameContext(DbContextOptions<HallOfFameContext> options)
            :base(options)
        { }

        public DbSet<Joke> Jokes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Joke>()
                .HasKey(joke => joke.Id);

            modelBuilder.Entity<User>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<Vote>()
                .HasKey(voteHistory => new { voteHistory.UserId, voteHistory.JokeId });

            modelBuilder.Entity<Vote>()
                .HasOne(vh => vh.Joke)
                .WithMany(joke => joke.Votes)
                .HasForeignKey(vh => vh.JokeId);
        }
    }

    public class Joke
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Vote
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int JokeId { get; set; }
        public Joke Joke { get; set; }
        public Int16 VoteState { get; set; }
    }
}
