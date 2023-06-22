﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Rating
{
    public class RatingViewModelWorkMapping
    {
        public string Id { get; set; } = null!;
        public string WorkingMappingId { get; set; }

        public double? RatingPoint { get; set; }

        public string? Comment { get; set; }

        public string? ImgUrl { get; set; }

        public string? Reciver { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
