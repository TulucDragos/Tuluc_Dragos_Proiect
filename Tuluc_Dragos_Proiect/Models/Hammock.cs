using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tuluc_Dragos_Proiect.Models
{
    public class Hammock
    {
        public int ID { get; set; }
        public string Nume { get; set; }
        public string Culoare { get; set; }
        public string Producator { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Pret { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<DistributedHammock> DistributedHammocks { get; set; }
    }
}
