using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shoaaa.DAL;
using Shoaaa.DAL.Entities;
using ShoaaFileViewer.Services;
using System.IdentityModel.Tokens.Jwt;

namespace ShoaaFileViewer.Controllers
{       

    public class RegisterationController : Controller

    {
        private static List<User> _users;

        public RegisterationController(List<User> user)
        {
            _users = user;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User newUser)
        {
            
            
            // Check for email exist
            var isEmailExists = _users.Exists(u => u.Email == newUser.Email);
            var isUsernameExists = _users.Exists(u => u.UserName == newUser.UserName);

            if (isEmailExists)
            {
                return Conflict(new { message = "Email already exists." });
            }

            if (isUsernameExists)
            {
                return Conflict(new { message = "Username already exists." });
            }
          

            
            _users.Add(newUser);

            return CreatedAtAction(nameof(Register), new { id = newUser.Id }, newUser);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

