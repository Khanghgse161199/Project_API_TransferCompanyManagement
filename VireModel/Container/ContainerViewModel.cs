using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Container
{
    public class ContainerViewModel
    {
        public string Id { get; set; } = null!;
        public string name { get; set; }
        public string CategoryName { get; set; } = null!;
        public bool IsWorking { get; set; }
        public double Weight { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
