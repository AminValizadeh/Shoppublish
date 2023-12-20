using Shop.Domain.Models.Order;
using Shop.Domain.ViewModels.Chart;
using Shop.Domain.ViewModels.Orders;

namespace Shop.Application.Interfaces
{
    public interface IOrderService
    {
        #region order

        Task<int> AddOrderForUser(int userId);
        Task<Order> GetUserLatestOpenOrder(int userId);
        Task<int> GetTotalOrderPriceForPayment(int userId);
        Task PayOrderProduct(int userId, int refId);
        Task<List<Order>> GetUserShoppingCartsByUserId(int userId);

        #endregion

        #region order detail

        Task AddProductToOpenOrder(int userId, AddProductToOrderViewModel order);
        Task<UserOpenOrderViewModel> GetUserOpenOrderDetail(int userId);
        Task<bool> RemoveOrderDetail(int detailId, int userId);
        Task ChangeOrderDetailCount(int detailId, int userId, int count);
        Task<int> GetNumberSalesInLastTenDays();
        Task<int> GetNumberSalesInCurrentMonthShamsi();
        Task<List<ChartBestSellerProductViewModel>> GetProductBestSellerLastTenDays();
        Task<List<ChartBestSellerProductViewModel>> GetProductBestSellerCurrentMonthShamsi();
        Task<List<OrderDetail>> GetDetailUserShoppingCartsByUserIdAndOrderId(int userId, int orderId);

        #endregion
    }
}
