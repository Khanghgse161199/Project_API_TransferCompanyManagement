using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.TransitCar
{
    public class TransitCarViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime DateRegister { get; set; }

        public string Name { get; set; } = null!;

        public string Brand { get; set; } = null!;      

        public string OriginCompany { get; set; } = null!;
        public DateTime OutOfDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
