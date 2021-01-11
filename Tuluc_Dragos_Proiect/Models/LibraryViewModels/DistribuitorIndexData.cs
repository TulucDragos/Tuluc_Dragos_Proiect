using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tuluc_Dragos_Proiect.Models.LibraryViewModels
{
    public class DistribuitorIndexData
    {
        public IEnumerable<Distribuitor> Distribuitors { get; set; }
        public IEnumerable<Hammock> Hammocks { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
