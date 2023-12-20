using Shop.Domain.Models.Order;
using Shop.Domain.Models.Product;

namespace Shop.Domain.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetAllOrderDetailByOrderId(int orderId);
        Task<OrderDetail> GetOrderDetailById(int id);
        Task AddOrderDetail(OrderDetail orderDetail);
        Task UpdateOrderDetail(OrderDetail orderDetail);
        Task SaveChange();
        Task<int> GetNumberSalesByDateTime(DateTime date);
        Task<List<OrderDetail>> GetOrderDetailsByDateTime(DateTime date);
        Task<List<OrderDetail>> GetDetailUserShoppingCartsByUserIdAndOrderId(int userId, int orderId);
    }
}
