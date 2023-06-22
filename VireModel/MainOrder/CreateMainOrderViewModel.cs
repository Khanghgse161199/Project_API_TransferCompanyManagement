using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Order
{
    public class CreateMainOrderViewModel
    {
        [Required]
        public string id { get; set; } = null!;
        [Required]
        public string Reciver { get; set; } = null!;
        [Required]
        public string ReciverCitizenId { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ReciverPhone { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string ReciverEmail { get; set; } = null!;
        [Required]
        public string ReciverAddress { get; set; } = null!;
        [Required]
        public string Sender { get; set; } = null!;
        [Required]
        public decimal Total { get; set; }
    }
}
