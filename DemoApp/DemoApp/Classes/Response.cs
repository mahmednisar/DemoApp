using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Classes
{
    public class Response
    {
        public int ResponseCode { get; set;  }
        public string  ResponseMessage{ get; set;  }
        public bool ResponseStatus { get; set;  }
        public object ResponseData { get; set;  }
    }
}
