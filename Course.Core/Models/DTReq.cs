using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Models
{
    public class DTReq
    {
        public int draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string OrderName { get; set; }
        public string OrderType { get; set; }



    }
}
