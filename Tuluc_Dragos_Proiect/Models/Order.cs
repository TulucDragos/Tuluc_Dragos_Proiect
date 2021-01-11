using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tuluc_Dragos_Proiect.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int HammockID { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public Hammock Hammock { get; set; }
    }
}
