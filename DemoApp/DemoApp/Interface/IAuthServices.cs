using DemoApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Interface
{
    public interface IAuthServices
    {
        Response Authenticate(LoginReqDTO loginReq);
    }
}
