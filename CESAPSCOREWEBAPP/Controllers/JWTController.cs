using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CESAPSCOREWEBAPP.Controllers
{
    [EnableCors("AllowAll")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : Controller
    {
        private IConfiguration Config { get; }
        private DatabaseContext Context { get; }

        public JWTController(IConfiguration config, DatabaseContext context)
        {
            this.Context = context;
            this.Config = config;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]Login login)
        {
            IActionResult response = Unauthorized();
            var Logins = Authenticate(login);
            if (Logins != null)
            {
                var tokenString = BuildToken(Logins);
                response = Ok(new { token = tokenString });
            }
            return response;

        }



        private string BuildToken(Login Logins)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(Config["Jwt:Expires"]));

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, Logins.Username),
                new Claim(JwtRegisteredClaimNames.Email,Logins.Password),
                new Claim("Myhost","MYHOSTCOMPANY"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                Config["Jwt:Issuer"],
                Config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private Login Authenticate(Login login)
        {
            Login users = Context.Logins.FirstOrDefault(
                x => x.Username.Equals(login.Username) && x.Password.Equals(login.Password));
            return users;
        }

    }
}