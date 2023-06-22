using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CategoryBlock
{
    public class CreateCategoryBlockViewModel
    {
        [Required]
        public string name { get; set; }
    }
}
