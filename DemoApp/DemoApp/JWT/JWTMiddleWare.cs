using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.JWT
{
    public class JWTMiddleWare
    {
        private RequestDelegate _next;
        public JWTMiddleWare(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext context)
        {
            var header = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (header != null)
            {
                new JwtSecurityTokenHandler().ValidateToken(header, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ffgjjjfsdshysdgfhgjkdsdsadsfdgfhghstdtfyguhiyt5465768rdyuyjhliyt7yuvg")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "name").Value;
                context.Items.Add("Name", name);
            }
            await _next(context);
        }
    }
}