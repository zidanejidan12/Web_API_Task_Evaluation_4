using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BusinessLogic.Interfaces;
using MyRESTServices.Domain.Models;
using MyRESTServices.Helpers;
using MyRESTServices.Models;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;

        public UserController(IUserBLL userBLL, IConfiguration configuration, JwtHelper jwtHelper) // Inject JwtHelper
        {
            _userBLL = userBLL;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _userBLL.GetUserWithRoles(loginDTO.Username); // Retrieve user with roles
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = user.Roles.Select(r => r.RoleName); // Assuming Roles is a collection of role names
            var token = _jwtHelper.GenerateJwtToken(user, roles); // Generate JWT token

            // Create an anonymous object with token, username, and roles to return in the response
            var response = new
            {
                Token = token,
                Username = user.Username,
                Roles = roles
            };

            return Ok(response);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    return BadRequest("Username is required.");
                }

                // Change the password
                var success = await _userBLL.ChangePassword(model.Username, model.NewPassword);
                if (!success)
                {
                    return BadRequest("Failed to change password.");
                }

                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while changing password: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userBLL.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userBLL.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
