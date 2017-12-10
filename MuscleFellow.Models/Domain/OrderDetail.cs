using System;

namespace MuscleFellow.Models.Domain
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public Guid OrderID { get; set; }

        public Guid ProductID { get; set; }

        public int Quantity { get; set; }

        public float UnitPrice { get; set; }

        public float SubTotal { get; set; }
        /// <summary>
        /// 本次订单的下单时间
        /// </summary>
        /// <value>The place date.</value>
        public DateTime? PlaceDate { get; set; }
    }
}