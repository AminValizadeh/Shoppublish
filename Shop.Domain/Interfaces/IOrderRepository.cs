using Shop.Domain.Models.Order;

namespace Shop.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrderByUserId(int userId);
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task SaveChange();
        Task<bool> IsExistUserOpenOrderByUserId(int userId);
        Task<Order> GetUserOpenOrderByUserId(int userId);
        Task<List<Order>> GetUserShoppingCartsByUserId(int userId);
    }
}
