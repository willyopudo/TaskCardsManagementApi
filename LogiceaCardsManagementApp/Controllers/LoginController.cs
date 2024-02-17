using Asp.Versioning;
using LogiceaCardsManagementApp2.CQRS.User_.Queries;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LogiceaCardsManagementApp2.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private IMediator mediator;
        private ILogger<LoginController> _logger;
        public LoginController(IConfiguration config, IMediator mediator, ILogger<LoginController> logger)
        {
            _config = config;
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] FindUserByEmailQuery query)
        {

            //If login usrename and password are correct then proceed to generate token
            try
            {
                User user = await mediator.Send(query);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, ((UserRoles)user.Role).ToString())
                    };
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      claims : claims,
                      null,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    _logger.LogInformation($"Token issued for User {user.Email}");

                    return Ok(token);
                }
                else {
                    return Ok("Email or Password incorrect");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"User login error error :: {ex.Message}");
            }

            return NotFound();
        }
    }
}
