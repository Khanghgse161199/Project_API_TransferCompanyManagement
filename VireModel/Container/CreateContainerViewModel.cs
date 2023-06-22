using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Container
{
    public class CreateContainerViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string CategoryTransId { get; set; } = null!;
        [Required]
        public double Weight { get; set; }
    }
}
