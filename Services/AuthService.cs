using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shoaaa.DAL.Entities;
using Shoaaa.DAL.JwtHelper;
using ShoaaFileViewer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace ShoaaFileViewer.Services
{
    public class AuthService
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase
        {
            [HttpPost("login")]
            public IActionResult Login([FromBody] Shoaaa.DAL.Dtos.LoginModelDto user)
            {
                if (user.Email != null && user.Password == "password");
                {
                    var token = GenerateJwtToken(user.UserName);
                    return Ok(new { token });
                }
                return Unauthorized();
            }

            private string GenerateJwtToken(string username)
            {
                var claims = new[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role,"ExternalUser"),
            new Claim(ClaimTypes.Role,"Manager")
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            [ApiController]
            [Route("api/[controller]")]
            public class ValuesController : ControllerBase
            {
                [HttpGet]
                [Authorize]
                public IActionResult GetValues()
                {
                    return Ok(new string[] { "value1", "value2" });
                }
            }
        }

    }
}
