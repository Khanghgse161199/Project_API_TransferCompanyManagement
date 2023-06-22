using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VireModel.EmployeeViewModel
{
    public class CreateEmployeeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        public string Address { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Text)]
        public string CitizenId { get; set; } = null!;
    }
}
