using AXL.Commons;
using AXL.Dto;
using AXL.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AXL.Api.Controllers
{
    [Authorize()]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<UserController> logger;
        private readonly IConfiguration configuration;
        public UserController(IUserService UserService,IHttpContextAccessor HttpContextAccessor,ILogger<UserController> Logger,IConfiguration Configuration)
        {
            userService = UserService;
            httpContextAccessor = HttpContextAccessor;
            logger = Logger;
            configuration = Configuration;
        }
        [HttpGet]
        public Task<List<UserDto>> GetUsers() {
            return userService.GetUsers();
        }
        [AllowAnonymous]
        [HttpGet]
        public string GetCilentIp() => httpContextAccessor!.HttpContext!.Connection!.RemoteIpAddress!.ToString();
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Login(string userName, string passWord)
        {
            var _key = configuration.GetSection("Secret").ToString();
            var Issuer = configuration.GetValue<string>("Issuer");
            var Audience = configuration.GetValue<string>("Audience");
            var res = await userService.Login(userName,passWord);
            if (res > 0)
            {

                //            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)), SecurityAlgorithms.HmacSha256);
                //            var claims = new Claim[] {
                //            new Claim(JwtRegisteredClaimNames.Iss,Issuer),
                //            new Claim(JwtRegisteredClaimNames.Aud,Audience),
                //            new Claim("Guid",Guid.NewGuid().ToString("D")),
                //            new Claim("name", userName),
                //            new Claim("password", passWord)
                //            };
                //            SecurityToken securityToken = new JwtSecurityToken(
                //    signingCredentials: securityKey,
                //    expires: DateTime.Now.AddMinutes(2),//过期时间
                //    claims: claims
                //);
                //            return new JwtSecurityTokenHandler().WriteToken(securityToken);

                var tokenHandle = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes(_key!);
                var toeknDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = Issuer,
                    Audience = Audience,
                    NotBefore = DateTime.Now,
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("Guid",Guid.NewGuid().ToString("D")),
                        new Claim("name", userName),
                        new Claim("password", passWord)
                    }),
                    Expires = DateTime.Now.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                     )
                };
                var token = tokenHandle.CreateToken(toeknDescriptor);
                return tokenHandle.WriteToken(token);
            }
            else
            {
                throw new BusinessException(400, "用户信息错误");
            }
        }
    }
}
