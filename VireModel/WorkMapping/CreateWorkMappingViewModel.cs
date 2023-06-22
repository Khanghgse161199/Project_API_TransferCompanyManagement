using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.WorkMapping
{
    public class CreateWorkMappingViewModel
    {       
        public string? EmployeeId { get; set; } = null!;
        [Required]
        public string TransitCarId { get; set; } = null!;
        [Required]
        public string ContainerId { get; set; } = null!;
    }

    public enum status
    {
        UnActive = 1,
        IsWorking = 2,
        Finish = 3
    }
}
