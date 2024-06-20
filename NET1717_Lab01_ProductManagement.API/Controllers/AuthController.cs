using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NET1717_Lab01_ProductManagement.API.Models.AuthModel;
using NET1717_Lab01_ProductManagement.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NET1717_Lab01_ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        public AuthController(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("sign-in")]
        public IActionResult SignIn(SignInModel signInModel)
        {
            var userLogin = _unitOfWork.UserRepository.Get(x => x.userName == signInModel.UserName && x.userPassword == signInModel.Password).entities.FirstOrDefault();
            if (userLogin != null)
            {
                var jwtTokenHandle = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Audience = _configuration["JWT:ValidAudience"],
                    Issuer = _configuration["JWT:ValidIssuer"],
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                    new Claim ("Name", userLogin.Name),
                    new Claim ("username", userLogin.userName),
                    new Claim (ClaimTypes.Role, userLogin.role),
                }),
                    IssuedAt = DateTime.Now,
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature),
                };


                var Token = jwtTokenHandle.CreateToken(tokenDescription);
                var accessToken = jwtTokenHandle.WriteToken(Token);
                return Ok(new TokenResponse
                {
                    token = accessToken,
                });
            }
            return BadRequest();
        }
    }
}
