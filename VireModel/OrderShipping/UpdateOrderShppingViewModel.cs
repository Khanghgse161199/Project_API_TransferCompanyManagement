using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.OrderDetail
{
    public class UpdateOrderShppingViewModel
    {
        public string OrderId { get; set; } = null!;
        public string BlockId { get; set; } = null!;
        public string WorkMappingId { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
