using System;
using System.Collections.Generic;

namespace MuscleFellow.Models.Domain
{
    public class Category
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        /// <value>The category identifier.</value>
        public int CategoryID { get; set; }
        /// <summary>
        /// 类型的正式名称
        /// </summary>
        /// <value>The name of the category.</value>
        public string CategoryName { get; set; }

        public List<Product> Products { get; set; }
    }
}
