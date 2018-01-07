using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MuscleFellow.Models.Domain;
using MuscleFellow.Web.Services;

namespace MuscleFellow.Web.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<CartItem> CartItems { get; set; }

        private readonly int _MaxCartItemCount = 30;

        private readonly ICartItemService _cartItemService;

        public IndexModel(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task OnGetAsync()
        {
            string sessionID = HttpContext.Session.Id;
            List<CartItem> cartList = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                // Get current user name
                string userID = HttpContext.User.Identity.Name;
                cartList = await _cartItemService.GetCartItemsAsync(sessionID, userID, _MaxCartItemCount, 0);
            }
            else
                cartList = await _cartItemService.GetCartItemsAsync(sessionID, _MaxCartItemCount, 0);
            CartItems = cartList;
        }
        //http://www.talkingdotnet.com/handle-ajax-requests-in-asp-net-core-razor-pages/
        public async Task<IActionResult> OnPostAsync(Guid productID, int amount) //
        {
            //Guid productID = Guid.Empty; int amount =0;
            // Get current user session id.
            string sessionID = HttpContext.Session.Id;
            // Get current user name
            string userID = "anonymous";
            if (HttpContext.User.Identity.IsAuthenticated)
                userID = HttpContext.User.Identity.Name;

            int count = await _cartItemService.AddAsync(sessionID, userID, productID, amount);
            JsonResult result = null;
            if (count > 0)
                result = new JsonResult(
                    new
                    {
                        success = true,
                        message = "商品已经被添加进<a href=\"/ShoppingCart\">购物车</a>",
                    });
            else
                result = new JsonResult(
                    new
                    {
                        success = true,
                        message = "添加购物车失败，请重试",
                    });
            return result;
        }
    }
}