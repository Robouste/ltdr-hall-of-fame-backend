using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ltdr_hall_of_fame_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace ltdr_hall_of_fame_backend.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Votes")]
    public class VotesController : Controller
    {
        private readonly HallOfFameContext _context;

        public VotesController(HallOfFameContext context)
        {
            _context = context;
        }

        // GET: api/Votes
        [HttpGet]
        public IEnumerable<Vote> GetVotes()
        {
            return _context.Votes;
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vote = await _context.Votes.SingleOrDefaultAsync(m => m.UserId == id);

            if (vote == null)
            {
                return NotFound();
            }

            return Ok(vote);
        }

        // PUT: api/Votes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote([FromRoute] int id, [FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vote.UserId)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Votes
        [HttpPost]
        public async Task<IActionResult> PostVote([FromBody] Vote vote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingVote = _context.Votes.SingleOrDefault(v => v.JokeId == vote.JokeId && v.UserId == vote.UserId);

            if (existingVote != null && existingVote.VoteState != vote.VoteState)
            {
                existingVote.VoteState = vote.VoteState;
                _context.Entry(existingVote).State = EntityState.Modified;
            }
            else if(existingVote == null)
            {
                _context.Votes.Add(vote);
            }
            else
            {
                return BadRequest("You can't vote twice the same choice");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoteExists(vote.UserId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetVote", new { id = vote.UserId }, vote);
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vote = await _context.Votes.SingleOrDefaultAsync(m => m.UserId == id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return Ok(vote);
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.UserId == id);
        }
    }
}