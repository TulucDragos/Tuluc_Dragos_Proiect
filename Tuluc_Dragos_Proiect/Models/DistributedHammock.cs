using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tuluc_Dragos_Proiect.Models
{
    public class DistributedHammock
    {
        public int DistribuitorID { get; set; }
        public int HammockID { get; set; }
        public Distribuitor Distribuitor { get; set; }
        public Hammock Hammock { get; set; }
    }
}
