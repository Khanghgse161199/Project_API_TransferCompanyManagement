using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.WorkMapping
{
    public class UpdateWorkMappingViewModel
    {
        [Required]
        public string EmployeeId { get; set; } = null!;     
    }
}
