using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ltdr_hall_of_fame_backend.Models;
using ltdr_hall_of_fame_backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ltdr_hall_of_fame_backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private readonly HallOfFameContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(HallOfFameContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RequestToken([FromBody] User request)
        {
            var user = _context.Users.SingleOrDefault(u => u.Name == request.Name);

            if (user == null)
            {
                return BadRequest("User name incorrect");
            }

            if (user.Password != request.Password)
            {
                return BadRequest("Password incorrect");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "hos.robouste.be",
                audience: "hos.robouste.be",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = AutoMapper.Mapper.Map<UserViewModel>(user)
            });
        }
    }

}