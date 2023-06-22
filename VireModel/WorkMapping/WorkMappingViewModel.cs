using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.WorkMapping
{
    public class WorkMappingViewModel
    {
        public string Id { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
       
        public string TransitCarId { get; set; } = null!;
       
        public string ContainerId { get; set; } = null!;

        public bool IsDone { get; set; }

        public double SummaryRating { get; set; }
 
        public DateTime DateTimeCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
