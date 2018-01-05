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
    }
}