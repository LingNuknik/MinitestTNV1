using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Models
{
    public class getProductDTRes : DTRes
    {
        public List<ProductDT> data { get; set; }
    }

    public class ProductDT
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Ranking { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double PriceNet  { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }


    }
}
