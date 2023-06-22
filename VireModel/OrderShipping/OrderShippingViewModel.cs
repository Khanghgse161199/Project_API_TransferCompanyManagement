using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.OrderDetail
{
    public class OrderShippingViewModel
    {
        public string Id { get; set; } = null!;
        public string MainOrderId { get; set; } = null!;
        public string BlockId { get; set; } = null!;
        public string WorkMappingId { get; set; } = null!;
        public string RatingId { get; set; } = null!;
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime? DateTimeRecive { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool IsDone { get; set; }
    }
}
