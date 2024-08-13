using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Models
{
    public class getProductDTReq : DTReq
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
    }
}
