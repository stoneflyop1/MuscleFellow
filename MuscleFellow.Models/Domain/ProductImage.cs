using System;
using System.ComponentModel.DataAnnotations;

namespace MuscleFellow.Models.Domain
{
    public class ProductImage
    {
        [Key]
        public int ImageID { get; set; }

        public Guid ProductID { get; set; }

        public string RelaticeUrl { get; set; }

        public string Comments { get; set; }
    }
}