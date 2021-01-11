using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tuluc_Dragos_Proiect.Models
{
    public class Distribuitor
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Publisher Name")]
        [StringLength(50)]
        public string DistributorName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<DistributedHammock> DistributedHammocks { get; set; }
    }
}
