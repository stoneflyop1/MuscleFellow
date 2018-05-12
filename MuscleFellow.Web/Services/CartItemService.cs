using Microsoft.EntityFrameworkCore;
using MuscleFellow.Data;
using MuscleFellow.Models;
using MuscleFellow.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuscleFellow.Web.Services
{
    public interface ICartItemService
    {
        Task<int> AddAsync(CartItem cartItem);

        Task<int> AddAsync(string sessionID, string userID, Guid productID, int amount);

        Task<int> DeleteAsync(Guid cartItemID);

        Task<int> UpdateAnonymousCartItem(string sessionID, string userID);

        Task<List<CartItem>> GetCartItemsAsync(string sessionID, string userID, int pageSize, int pageCount);

        Task<List<CartItem>> GetCartItemsAsync(string sessionID, int pageSize, int pageCount);

        Task<int> UpdateAsync(CartItem cartItem);

        Task<CartItem> GetByID(Guid id);
    }

	#pragma warning disable 1591

    public class CartItemService : ICartItemService
    {
        private readonly IRepository<CartItem> _cartRepo = null;
        private readonly IRepository<Product> _prodRepo;
        private readonly int _MaxCartItemCount = 30;
        public CartItemService(IRepository<CartItem> cartRepo, IRepository<Product> prodRepo)
        {
            _cartRepo = cartRepo;
            _prodRepo = prodRepo;
        }

        public async Task<int> AddAsync(CartItem cartItem)
        {
            if (cartItem.UserID == "anonymous")
            {
                if (_cartRepo.Table.Count(c => c.SessionID == cartItem.SessionID) > _MaxCartItemCount)
                    return 0;
            }
            else
            {
                if (_cartRepo.Table.Count(c => c.UserID == cartItem.UserID) > _MaxCartItemCount)
                    return 0;
            }
            CartItem existedCartItem = null;
            if (cartItem.UserID == "anonymous")
                existedCartItem = await _cartRepo.FirstOrDefaultAsync(
                    c => c.SessionID == cartItem.SessionID && c.ProductID == cartItem.ProductID);
            else
                existedCartItem = await _cartRepo.FirstOrDefaultAsync(
                    c => c.UserID == cartItem.UserID && c.ProductID == cartItem.ProductID);
            if (null != existedCartItem)
            {
                existedCartItem.Quantity += cartItem.Quantity;
                existedCartItem.ProductName = cartItem.ProductName;
                existedCartItem.ThumbImagePath = cartItem.ThumbImagePath;
                existedCartItem.UnitPrice = cartItem.UnitPrice;
                existedCartItem.SubTotal = existedCartItem.Quantity * existedCartItem.UnitPrice;
                return await _cartRepo.UpdateAsync(existedCartItem);
            }
            else
                return await _cartRepo.InsertAsync(cartItem);
        }
        public async Task<int> AddAsync(string sessionID, string userID, Guid productID, int amount)
        {
            Product prod = await _prodRepo.Table.Where(
                p => p.ProductID == productID).SingleAsync();
            if (null == prod)
                return 0;
            CartItem item = new CartItem();
            item.CartID = Guid.NewGuid();
            item.UserID = userID;
            item.ProductID = productID;
            item.ProductName = prod.ProductName;
            item.ThumbImagePath = prod.ThumbnailImage;
            item.SessionID = sessionID;
            item.CreatedDateTime = DateTime.Now;
            item.LastUpdatedDateTime = item.CreatedDateTime;
            item.Quantity = amount;
            item.UnitPrice = prod.UnitPrice;
            item.SubTotal = prod.UnitPrice * amount;

            return await AddAsync(item);
        }
        public async Task<int> DeleteAsync(Guid cartItemID)
        {
            var cartItem = await _cartRepo.FirstOrDefaultAsync(c => c.CartID == cartItemID);
            if (null != cartItem)
            {
                return await _cartRepo.DeleteAsync(cartItem);
            }
            return 0;
        }
        public async Task<int> UpdateAnonymousCartItem(string sessionID, string userID)
        {
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(sessionID))
                return 0;
            List<CartItem> items = _cartRepo.Table.Where(c => c.SessionID == sessionID).ToList();
            if (items.Count > 0)
            {
                foreach (CartItem item in items)
                {
                    item.UserID = userID;
                }
                return await _cartRepo.UpdateAsync(items);
            }
            return 0;
        }
        public async Task<List<CartItem>> GetCartItemsAsync(string sessionID, string userID, int pageSize, int pageCount)
        {
            var results = await _cartRepo.Table.Where(c => c.UserID == userID || c.SessionID == sessionID)
                   .Select(c => new { CartItem = c, })
                   .OrderByDescending(c => c.CartItem.CreatedDateTime)
                   .Skip(pageSize * pageCount)
                   .Take(pageSize)
                   .ToListAsync();
            return results.Select(c => c.CartItem).ToList();
        }
        public async Task<List<CartItem>> GetCartItemsAsync(string sessionID, int pageSize, int pageCount)
        {
            var results = await _cartRepo.Table.Where(c => c.SessionID == sessionID)
                   .Select(c => new { CartItem = c, })
                   .OrderByDescending(c => c.CartItem.CreatedDateTime)
                   .Skip(pageSize * pageCount)
                   .Take(pageSize)
                   .ToListAsync();
            return results.Select(c => c.CartItem).ToList();
        }
        public async Task<int> UpdateAsync(CartItem cartItem)
        {
            if (null == cartItem)
                return -1;
            return await _cartRepo.UpdateAsync(cartItem);
        }
        public async Task<CartItem> GetByID(Guid id)
        {
            return await _cartRepo.FirstOrDefaultAsync(c => c.CartID == id);
        }
    }
}
