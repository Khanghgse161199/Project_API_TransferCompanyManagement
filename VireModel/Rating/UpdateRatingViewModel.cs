using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Rating
{
    public class UpdateRatingViewModel
    {
        [Required]
        public double RatingPoint { get; set; }

        public string? Comment { get; set; }

        public string? ImgUrl { get; set; }
        [Required]
        public string Reciver { get; set; }
    }
}
