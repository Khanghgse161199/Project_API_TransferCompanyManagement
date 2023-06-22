using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.EmployeeViewModel
{
    public class EmployeeUpdateProfileViewModel
    {
        [DataType(DataType.Text)]
        public string FullName { get; set; } = null!;
        [DataType(DataType.Text)]
        public string Address { get; set; } = null!;
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.Text)]
        public string CitizenId { get; set; } = null!;
    }
}
