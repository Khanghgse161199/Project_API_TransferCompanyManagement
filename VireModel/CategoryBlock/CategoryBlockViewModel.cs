using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CategoryBlock
{
    public class CategoryBlockViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
        public DateTime LastUpdate { get; set; }
    }
}
