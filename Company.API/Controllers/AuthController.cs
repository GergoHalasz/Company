using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Company.API.DTOs;

namespace Company.API.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto loginDto)
		{
			// Validate user credentials (this should be done with a real authentication mechanism, like checking a database)
			if (loginDto.Username != "testUser" || loginDto.Password != "testPassword")  // Example validation
			{
				return Unauthorized("Invalid credentials");
			}

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, loginDto.Username), // Add user claims
                new Claim(ClaimTypes.Role, "Admin")           // You can also add roles or other claims
            };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return Ok(new { Token = tokenString });  // Return the JWT token to the client
		}
	}
}
