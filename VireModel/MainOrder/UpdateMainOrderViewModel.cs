using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.OrderShipping
{
    public class UpdateMainOrderViewModel
    {
        public string Reciver { get; set; } = null!;
        public string ReciverCitizenId { get; set; } = null!;
        [DataType(DataType.PhoneNumber)]
        public string ReciverPhone { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string ReciverEmail { get; set; } = null!;
        public string ReciverAddress { get; set; } = null!;
        public string Sender { get; set; } = null!;
        public decimal Total { get; set; }
    }
}
