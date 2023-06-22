using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.TransitCar
{
    public class UpdateTransitCarViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string brand { get; set; }
        [Required]
        public string OriginCompany { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime registerDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime outOfDate { get; set; }
    }
}
