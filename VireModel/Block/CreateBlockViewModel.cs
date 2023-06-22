using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Block
{
    public class CreateBlockViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public double Weight { get; set; }
        [Required]
        public string CategoryBlockId { get; set; } = null!;
    }
}
