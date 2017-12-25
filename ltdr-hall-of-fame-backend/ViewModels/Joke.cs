using ltdr_hall_of_fame_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ltdr_hall_of_fame_backend.ViewModels
{
    public class JokeViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public string createdBy { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public int UpVotes => Votes.Where(vote => vote.VoteState == 1).Count();
        public int DownVotes => Votes.Where(vote => vote.VoteState == -1).Count();
    }
}
