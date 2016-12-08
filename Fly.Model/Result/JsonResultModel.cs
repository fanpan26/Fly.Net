using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model.Result
{
    public class JsonResultModel
    {
        public int status { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }
}
