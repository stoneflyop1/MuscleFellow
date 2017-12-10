using System;
namespace MuscleFellow.Models.Domain
{
    public class CartItem
    {
        /// <summary>
        /// 购物车项目的唯一ID
        /// </summary>
        /// <value>The cart identifier.</value>
        public Guid CartID { get; set; }
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserID { get; set; }
        /// <summary>
        /// 当前用户会话的唯一标识
        /// </summary>
        /// <value>The session identifier.</value>
        public string SessionID { get; set; }
        /// <summary>
        /// 商品的唯一ID
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductID { get; set; }
        /// <summary>
        /// 商品的正式名称
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品的缩略图片相对路径URL
        /// </summary>
        /// <value>The thumb image path.</value>
        public string ThumbImagePath { get; set; }
        /// <summary>
        /// 商品的购买数量
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }
        /// <summary>
        /// 商品的单价
        /// </summary>
        /// <value>The unit price.</value>
        public float UnitPrice { get; set; }
        /// <summary>
        /// 小计，即商品单价乘以购买数量
        /// </summary>
        /// <value>The sub total.</value>
        public float SubTotal { get; set; }
        /// <summary>
        /// 购物车项目最后更新的时间
        /// </summary>
        /// <value>The last updated date time.</value>
        public DateTime LastUpdatedDateTime { get; set; }
        /// <summary>
        /// 购物车项目创建时间
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }
    }
}
