using UserApi.Model;
using UserApi.Repository;
using UserApi.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace JWTAuth.WebApi.Controllers
{
    //[Route("api/token")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        //private readonly DatabaseContext _context;

        //public UserController(IConfiguration config, DatabaseContext context)
        //{
        //    _configuration = config;
        //    _context = context;
        //}
        private readonly IUserRepository<UserInfo> _userRpository;

        public UserController(IConfiguration config, IUserRepository<UserInfo> userRpository)
        {
            _configuration = config;
            _userRpository = userRpository;
        }
        [Route("api/user")]
        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = _userRpository.Find(id);

                if (user != null)
                {

                    return Ok(user);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/token")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserInfo _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = /*await*/ GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [AllowAnonymous]
        [Route("api/register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserApi.ViewModel.RegisterVM registerVM)
        {
            if (registerVM != null && registerVM.Email != null && registerVM.Password != null)
            {
                var user = /*await*/ GetUser(registerVM.Email, registerVM.Password);

                if (user == null)
                {
                    _userRpository.Add(registerVM.Map());

                    return Ok(user);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private /*async*/ /*Task<UserInfo>*/ UserInfo? GetUser(string email, string password)
        {

            return  _userRpository.Login(new LoginVM() {Email=email, Password=password});
            //return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}