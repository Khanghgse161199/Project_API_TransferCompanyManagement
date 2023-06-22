﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.WorkMapping
{
    public class RegisterEmployeeWorkingMappingViewModel
    {
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string WorkMappingId { get; set; }
    }
}
