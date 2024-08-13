using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Core.Models
{
    public class getCategoryDTRes : DTRes
    {
        public List<CategoryDT> data {  get; set; }
    }

    public class CategoryDT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Ranking { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
     

    }
}
