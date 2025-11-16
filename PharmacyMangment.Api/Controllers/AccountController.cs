using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pharmacy.Dto.UserDto;
using Pharmacy.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        UserManager<UserApp> _manager;
        IConfiguration _config;

        public AccountController(UserManager<UserApp> manager , IConfiguration config)
        {
            _manager = manager;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register (RegisterDto registerdto)
        {
            if (registerdto == null)
                return BadRequest("user == null");
            if(!ModelState.IsValid)
                return BadRequest("ModelState invalid");

            var user = await _manager.FindByEmailAsync(registerdto.Email);

            if (user != null)
                return BadRequest("User Already Exit");


            UserApp user1 = new UserApp()
            {
                FirstName = registerdto.FirstName,
                LastName = registerdto.LastName,
                Email = registerdto.Email,
                UserName= registerdto.Email
            };

           var result =  await _manager.CreateAsync(user1, registerdto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return Created();

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login (LoginDto Logindto)
        {
            if (Logindto == null)
                return BadRequest("user == null");
            if (!ModelState.IsValid)
                return BadRequest("ModelState invalid");


            var user = await _manager.FindByEmailAsync(Logindto.Email);
            if (user == null)
                return BadRequest("Invalid Email");

            var password = await _manager.CheckPasswordAsync(user,Logindto.Password);

            if(!password)
                return BadRequest("Invalid Password");

           var  Token = GenerateToken(user);

            return Ok (Token);
        }

        private string GenerateToken(UserApp user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name, user.FirstName),
                new Claim (ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.NameIdentifier,user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["jwt:key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                issuer : _config["jwt:issuer"],
                audience: _config["jwt:audiunce"],
                expires: DateTime.Now.AddHours(2),
                signingCredentials :new SigningCredentials(key ,SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
