using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ltdr_hall_of_fame_backend.Models;
using ltdr_hall_of_fame_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ltdr_hall_of_fame_backend.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly HallOfFameContext _context;

        public UsersController(HallOfFameContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<UserViewModel> GetUsers()
        {
            var test = User;
            return _context.Users.Select(user => AutoMapper.Mapper.Map<UserViewModel>(user));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(AutoMapper.Mapper.Map<UserViewModel>(user));
        }

        [HttpPut("{id}/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = _context.Users.SingleOrDefault(u => u.Name == User.Identity.Name);

            if (currentUser.Id != id)
            {
                return BadRequest("Tu ne peux update que ton propre password");
            }

            currentUser.Password = user.Password;

            _context.Entry(currentUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Users/5
        //[HttpPut("{id}")]
        //[Authorize(Roles = "gourou,executeur")]
        //public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "gourou,executeur")]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Password = "098f6bcd4621d373cade4e832627b4f6";
            user.Role = "disciple";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "gourou,executeur")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("{id}/Promote")]
        [Authorize(Roles = "gourou,executeur")]
        public async Task<IActionResult> PromoteUser([FromRoute] int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Role == "gourou" || user.Role == "executeur")
            {
                return BadRequest("Bien essayé");
            }

            if (user.Password == "098f6bcd4621d373cade4e832627b4f6")
            {
                return BadRequest("L'utilisateur doit d'abord changer son mot de passe");
            }

            user.Role = "executeur";

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpGet("{id}/Demote")]
        [Authorize(Roles = "gourou,executeur")]
        public async Task<IActionResult> DemoteUser([FromRoute] int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Role == "gourou")
            {
                return BadRequest("Bien essayé");
            }

            user.Role = "disciple";

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}