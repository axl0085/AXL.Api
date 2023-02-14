using AXL.Dto;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace AXL.Api.Unit
{
    public static class IHttpContextAccessorException
    {
        public static UserDto GetCuurentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token").Result;
            var handler = new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(token).Payload;
            var claims = payload.Claims;
            var UserName = claims.First(claim => claim.Type == "UserName").Value;
            return new UserDto() { UserName = UserName };
        }
    }
}