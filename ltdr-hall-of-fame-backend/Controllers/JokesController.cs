﻿using System;
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
    [Route("api/Jokes")]
    public class JokesController : Controller
    {
        private readonly HallOfFameContext _context;

        public JokesController(HallOfFameContext context)
        {
            _context = context;
        }

        // GET: api/Jokes
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Joke> GetJokes()
        {
            var result = _context.Jokes
                .Include(joke => joke.Votes)
                    .ThenInclude(vote => vote.User)
                .OrderByDescending(joke => joke.Id);

            return result;
        }

        // GET: api/Jokes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var joke = await _context.Jokes.SingleOrDefaultAsync(m => m.Id == id);

            if (joke == null)
            {
                return NotFound();
            }

            return Ok(joke);
        }

        // PUT: api/Jokes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJoke([FromRoute] int id, [FromBody] Joke joke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != joke.Id)
            {
                return BadRequest();
            }

            _context.Entry(joke).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JokeExists(id))
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

        // POST: api/Jokes
        [HttpPost]
        public async Task<IActionResult> PostJoke([FromBody] Joke joke)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.SingleOrDefault(u => u.Name == User.Identity.Name);

            if (user == null)
            {
                return BadRequest("Utilisateur non connecté");
            }

            if (user.Password == "098f6bcd4621d373cade4e832627b4f6")
            {
                return BadRequest("Tu dois d'abord changer ton mot de passe si tu veux ajouter une blague !");
            }

            joke.createdAt = DateTime.Now;
            joke.createdBy = user.Name;

            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoke", new { id = joke.Id }, joke);
        }

        // DELETE: api/Jokes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "gourou,executeur")]
        public async Task<IActionResult> DeleteJoke([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var joke = await _context.Jokes.SingleOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }
            var votes = _context.Votes.Where(vote => vote.JokeId == joke.Id);

            if (votes != null)
            {
                _context.Votes.RemoveRange(votes);
            }

            _context.Jokes.Remove(joke);
            await _context.SaveChangesAsync();

            return Ok(joke);
        }

        private bool JokeExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}