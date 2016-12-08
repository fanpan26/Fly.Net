using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Result
{
    public class RegisterResult
    {
        public string msg { get; set; }
        public bool success { get; set; }

        public User user { get; set; }
    }
}
