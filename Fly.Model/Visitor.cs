using Fly.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Model
{
   public class Visitor
    {
        public int MainId { get; set; }
        public int VisitorId { get; set; }
        public VisitorType VisitorType { get; set; }
    }
}
