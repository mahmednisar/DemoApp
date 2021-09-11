using DemoApp.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.JWT
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items.Count==0 || context.HttpContext.Items["Name"] == null || context.HttpContext.Items["Name"].ToString() == "")
            {
                context.Result = new JsonResult(new Response { ResponseCode = StatusCodes.Status401Unauthorized, ResponseStatus = false, ResponseMessage = "Unauthorized.." }) { StatusCode = StatusCodes.Status401Unauthorized, Value = null };
            }
           
        }
    }
}
