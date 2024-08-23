using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wordle.Api.Dtos;
using Wordle.Api.Identity;
using Wordle.Api.Models;

namespace Wordle.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    public AppDbContext _context;
    public UserManager<AppUser> _userManager;
    public JwtConfiguration _jwtConfiguration;
    public RoleManager<IdentityRole> _roleManager;
    public TokenController(AppDbContext context, UserManager<AppUser> userManager, JwtConfiguration jwtConfiguration, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
        _roleManager = roleManager;
    }

    [HttpPost("GetToken")]
    public async Task<IActionResult> GetToken([FromBody] UserCredentialsDto userCredentials)
    {
        if (string.IsNullOrEmpty(userCredentials.Email))
        {
            return BadRequest("Username is required");
        }
        if (string.IsNullOrEmpty(userCredentials.Password))
        {
            return BadRequest("Password is required");
        }

        var user = await _userManager.FindByEmailAsync(userCredentials.Email);

        if (user is null) { return Unauthorized("The user account was not found"); }

        bool results = await _userManager.CheckPasswordAsync(user, userCredentials.Password);
        if (results)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("userId", user.Id.ToString()),
                new("userName", user.UserName!.ToString()),
            };

            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims.Where(claim => claim.Type != null && claim.Value != null)
                .Select(claim => new Claim(claim.Type, claim.Value)));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var roleClaims = await _roleManager.GetClaimsAsync(
                    _roleManager.Roles.First(f => f.Name == role)
                );

                claims.AddRange(roleClaims
                    .Where(claim => claim.Type != null && claim.Value != null)
                    .Select(claim => new Claim(claim.Type, claim.Value))
                );
            }

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfiguration.ExpirationInMinutes),
                signingCredentials: credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = jwtToken });
        }
        return Unauthorized("The username or password is incorrect");
    }
}

