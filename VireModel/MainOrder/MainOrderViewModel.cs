using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Order
{
    public class MainOrderViewModel
    {
        public string Id { get; set; } = null!;
        public string Reciver { get; set; } = null!;
        public string ReciverCitizenId { get; set; } = null!;

        public string ReciverPhone { get; set; } = null!;

        public string ReciverEmail { get; set; } = null!;

        public string ReciverAddress { get; set; } = null!;

        public DateTime DateTimeCreate { get; set; }
        public decimal Total { get; set; }
        public double SummaryRating { get; set; }

        public bool IsWorking { get; set; }

        public string Sender { get; set; } = null!;
        public DateTime LastUpdate { get; set; }
    }
}
