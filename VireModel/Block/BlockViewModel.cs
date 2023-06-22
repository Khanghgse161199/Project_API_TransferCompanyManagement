using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Block
{
    public class BlockViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Weight { get; set; }
        public string CategoryBlockId { get; set; } = null!;
        public DateTime DateTimeCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
