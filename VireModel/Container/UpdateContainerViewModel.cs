using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Container
{
    public class UpdateContainerViewModel
    {
        public string name { get; set; }
        public string CategoryTransId { get; set; }
        public double Weight { get; set; }
    }
}
