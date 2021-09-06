using DemoApp.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
namespace DemoApp.Controllers
{
    public class HomeController : BaseController
    {

        Response response = new Response();
        public HomeController()
        {

        }

        [HttpGet]
        public Response GetToken(string Name)
        {
            var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "https://localhost:44302",
                IssuedAt = DateTime.Now,
                Subject = new ClaimsIdentity(new[] { new Claim("name", Name) }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ffgjjjfsdshysdgfhgjkdsdsadsfdgfhghstdtfyguhiyt5465768rdyuyjhliyt7yuvg")), SecurityAlgorithms.HmacSha256Signature)
            }));

            response.ResponseCode = StatusCodes.Status200OK;
            response.ResponseMessage = "";
            response.ResponseStatus = true;
            response.ResponseData = new { Token = token };
            return response;
        }


        [HttpGet]
        public Response GETData(string Token)
        {
           

            response.ResponseCode = StatusCodes.Status200OK;
            response.ResponseMessage = "";
            response.ResponseStatus = true;
            response.ResponseData = new { name = "" };
            return response;
        }

    }
}
