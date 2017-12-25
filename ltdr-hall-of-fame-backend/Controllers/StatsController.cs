using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ltdr_hall_of_fame_backend.Models;
using ltdr_hall_of_fame_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ltdr_hall_of_fame_backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Stats")]
    public class StatsController : Controller
    {
        private readonly HallOfFameContext _context;

        public StatsController(HallOfFameContext context)
        {
            _context = context;
        }

        [HttpGet]
        public StatsViewModel GetStats()
        {
            var jokesContext = _context.Jokes.Include(joke => joke.Votes).ToList();
            var jokes = AutoMapper.Mapper.Map<List<JokeViewModel>>(jokesContext);

            var bestJoke = jokes.Aggregate((j1, j2) => j1.UpVotes > j2.UpVotes ? j1 : j2);
            var worstJoke = jokes.Aggregate((j1, j2) => j1.DownVotes > j2.DownVotes ? j1 : j2);

            var bestAuthor = jokes.Where(joke => joke.UpVotes > joke.DownVotes)
                                .GroupBy(j => j.Author)
                                .OrderByDescending(grp => grp.Count())
                                .FirstOrDefault()?.Key;

            var worstAuthor = jokes.Where(joke => joke.DownVotes > joke.UpVotes)
                                .GroupBy(j => j.Author)
                                .OrderByDescending(grp => grp.Count())
                                .FirstOrDefault()?.Key;

            StatsViewModel result = new StatsViewModel()
            {
                BestAuthor = bestAuthor,
                BestJoke = bestJoke,
                WorstAuthor = worstAuthor,
                WorstJoke = worstJoke
            };

            return result;
        }
    }
}