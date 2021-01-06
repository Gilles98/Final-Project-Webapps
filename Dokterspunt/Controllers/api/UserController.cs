
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dokterspunt.Areas.Identity.Data;
using Dokterspunt.Helpers;
using Dokterspunt.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloCore.Controllers.api
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<LoggedInUser> _signInManager;
        private readonly UserManager<LoggedInUser> _userManager;
        private readonly AppSettings _appSettings;

        public UserController(
            UserManager<LoggedInUser> userManager,
            SignInManager<LoggedInUser> signInManager,
            IOptions<AppSettings> appSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }


        [HttpPost("authenticate")]
        public async Task<object> Authenticate([FromBody] ApiUser apiUser)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                apiUser.Username, apiUser.Password, false, false);

            if (signInResult.Succeeded)
            {
                LoggedInUser user = _userManager.Users.SingleOrDefault(r => r.UserName == apiUser.Username);
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    await _signInManager.SignOutAsync();
                    return BadRequest(new { message = "Deze user is niet de admin, enkel de admin mag in deze API rechten hebben" });
                }
                apiUser.Token = GenerateJwtToken(apiUser.Username, user).ToString();

                return apiUser;
            }

            return BadRequest(new { message = "Username or password is incorrect" });
        }

        private string GenerateJwtToken(string email, LoggedInUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}