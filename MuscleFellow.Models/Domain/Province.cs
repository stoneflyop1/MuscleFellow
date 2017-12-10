using System;
using System.Collections.Generic;

namespace MuscleFellow.Models.Domain
{
    public class Province
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public List<City> Cities { get; set; }
    }
}
