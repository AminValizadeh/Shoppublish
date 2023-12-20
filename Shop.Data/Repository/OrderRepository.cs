using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Order;

namespace Shop.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {

        #region construtor

        private readonly ShopContext _context;

        public OrderRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddOrder(Order order)
        {
            order.CreateDate = DateTime.Now;
            await _context.Orders.AddAsync(order);
            await SaveChange();
        }

        public async Task<List<Order>> GetAllOrderByUserId(int userId)
        {
            return await _context.Orders.Where(o=> o.UserId == userId).ToListAsync();
        }

       
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order> GetUserOpenOrderByUserId(int userId)
        {
            return await _context.Orders
                .Include(o=> o.OrderDetails)
                .ThenInclude(s => s.Product)
                .ThenInclude(s => s.ProductDiscounts)
                .SingleOrDefaultAsync(s => s.UserId == userId && !s.IsPaid);
        }

        public async Task<List<Order>> GetUserShoppingCartsByUserId(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId && o.IsPaid && !o.IsDelete).ToListAsync();
        }

        public async Task<bool> IsExistUserOpenOrderByUserId(int userId)
        {
            return await _context.Orders.AnyAsync(o => o.UserId == userId && !o.IsPaid);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await SaveChange();
        }
    }
}
