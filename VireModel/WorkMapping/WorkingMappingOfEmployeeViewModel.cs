using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.WorkMapping
{
    public class WorkingMappingOfEmployeeViewModel
    {      
        public string TransitCarId { get; set; } = null!;

        public string ContainerId { get; set; } = null!;

        public status IsWork { get; set; }

        public double SummaryRating { get; set; }

        public DateTime DateTimeCreate { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
