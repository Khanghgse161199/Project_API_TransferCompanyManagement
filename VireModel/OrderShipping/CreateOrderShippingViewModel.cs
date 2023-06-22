using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.OrderDetail
{
    public class CreateOrderShippingViewModel
    {
        [Required]
        public string OrderId { get; set; } = null!;
        [Required]
        public string BlockId { get; set; } = null!;
        [Required]
        public string WorkMappingId { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
    }
}
