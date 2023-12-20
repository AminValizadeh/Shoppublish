using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Order;
using Shop.Domain.ViewModels.Chart;
using Shop.Domain.ViewModels.Orders;

namespace Shop.Application.Services
{
    public class OrderService : IOrderService
    {
        #region constructor

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IWalletService _walletService;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IWalletService walletService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _walletService = walletService;
            _productService = productService;
        }

        #endregion

        #region order

        public async Task<int> AddOrderForUser(int userId)
        {
            var order = new Order { UserId = userId };

            await _orderRepository.AddOrder(order);

            return order.Id;
        }

        public async Task<Order> GetUserLatestOpenOrder(int userId)
        {
            if (!await _orderRepository.IsExistUserOpenOrderByUserId(userId))
                await AddOrderForUser(userId);

            return await _orderRepository.GetUserOpenOrderByUserId(userId);
        }

        public async Task<int> GetTotalOrderPriceForPayment(int userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            int totalPrice = 0;


            foreach (var detail in userOpenOrder.OrderDetails.Where(s => !s.IsDelete))
            {
                int discount = 0;
                if (detail.DiscountCode != null)
                    discount = await _productService.GetProductDiscountAmount(detail.DiscountCode, detail.ProductId, detail.Product.Price) ?? 0;

                totalPrice += detail.Count * (detail.Product.Price - discount);
            }

            return totalPrice;
        }

        public async Task PayOrderProduct(int userId, int refId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            int finalPrice = 0;
            int finalDiscount = 0;
            int finalPriceWithDiscount = 0;

            foreach (var detail in openOrder.OrderDetails)
            {
                var discount = 0;
                var productPrice = detail.Product.Price;
                var totalPrice = detail.Count * productPrice;

                if (detail.DiscountCode != null)
                    discount = await _productService.GetProductDiscountAmount(detail.DiscountCode, detail.ProductId, productPrice) ?? 0;

                var totalPriceWithDiscount = totalPrice - (discount * detail.Count);

                finalPrice += totalPrice;
                finalDiscount += discount * detail.Count;
                finalPriceWithDiscount += totalPriceWithDiscount;

                detail.ProductPrice = productPrice;
                detail.ProductDiscount = discount;
                detail.ProductPriceWithDiscount = totalPriceWithDiscount;
                await _orderDetailRepository.UpdateOrderDetail(detail);
            }

            openOrder.FinalPrice = finalPrice;
            openOrder.FinalDiscount = finalDiscount;
            openOrder.FinalPriceWithDiscount = finalPriceWithDiscount;
            openOrder.IsPaid = true;
            openOrder.TracingCode = refId.ToString();
            await _orderRepository.UpdateOrder(openOrder);
        }

        public async Task<List<Order>> GetUserShoppingCartsByUserId(int userId)
        {
            return await _orderRepository.GetUserShoppingCartsByUserId(userId);
        }

        #endregion

        #region order detail

        public async Task AddProductToOpenOrder(int userId, AddProductToOrderViewModel order)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var similarOrder = openOrder.OrderDetails.SingleOrDefault(s => s.ProductId == order.ProductId && !s.IsDelete);

            if (similarOrder == null)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = openOrder.Id,
                    ProductId = order.ProductId,
                    Count = order.Count,
                    DiscountCode = order.DiscountCode,
                };

                await _orderDetailRepository.AddOrderDetail(orderDetail);
            }
            else
            {
                similarOrder.Count = order.Count;
                similarOrder.DiscountCode = order.DiscountCode;
                await _orderDetailRepository.SaveChange();
            }
        }

        public async Task<UserOpenOrderViewModel> GetUserOpenOrderDetail(int userId)
        {
            Order userOpenOrder = await GetUserLatestOpenOrder(userId);

            return new UserOpenOrderViewModel
            {
                UserId = userId,
                Details = userOpenOrder.OrderDetails
                .Where(s => !s.IsDelete)
                .Select(s => new UserOpenOrderDetailItemViewModel
                {
                    DetailId = s.Id,
                    Count = s.Count,
                    ProductId = s.ProductId,
                    ProductPrice = s.Product.Price,
                    ProductTitle = s.Product.Title,
                    ProductImageName = s.Product.ImageName,
                    DiscountCode = s.DiscountCode,
                    DiscountPercentage =
                        (s.DiscountCode != null ? _productService.GetPercentageDiscount(s.DiscountCode, s.ProductId).Result : null)
                }).ToList()
            };
        }

        public async Task<bool> RemoveOrderDetail(int detailId, int userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var orderDetail = openOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
            if (orderDetail == null) return false;

            orderDetail.IsDelete = true;
            await _orderDetailRepository.UpdateOrderDetail(orderDetail);

            return true;
        }

        public async Task ChangeOrderDetailCount(int detailId, int userId, int count)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            var detail = userOpenOrder.OrderDetails.SingleOrDefault(s => s.Id == detailId);
            if (detail != null)
            {
                if (count > 0)
                {
                    detail.Count = count;
                }
                else
                {
                    detail.IsDelete = true;
                }
                await _orderDetailRepository.UpdateOrderDetail(detail);
                await _orderDetailRepository.SaveChange();
            }
        }

        public async Task<int> GetNumberSalesInLastTenDays()
        {
            DateTime dateTime = DateTime.Today.AddDays(-10);
            return await _orderDetailRepository.GetNumberSalesByDateTime(dateTime);
        }

        public async Task<int> GetNumberSalesInCurrentMonthShamsi()
        {
            var dateTimeShamsi = DateTime.Now.ToShamsiDateTime();
            var dateTimeMiladi = new DateTime(dateTimeShamsi.Year, dateTimeShamsi.Month, 1).ToMiladi();
            return await _orderDetailRepository.GetNumberSalesByDateTime(dateTimeMiladi);
        }

        public async Task<List<ChartBestSellerProductViewModel>> GetProductBestSellerLastTenDays()
        {
            DateTime dateTime = DateTime.Today.AddDays(-10);
            var res = await _orderDetailRepository.GetOrderDetailsByDateTime(dateTime);
            var duplicateProductId = res.Select(o => o.ProductId).Distinct().ToList();

            Dictionary<int, int> productIdWithNumberSales = new Dictionary<int, int>();
            foreach (var productId in duplicateProductId)
            {
                productIdWithNumberSales.Add(productId, res.Where(s => s.ProductId == productId).Sum(s => s.Count));
            }

            List<ChartBestSellerProductViewModel> product = new List<ChartBestSellerProductViewModel>();
            foreach (var item in productIdWithNumberSales.OrderByDescending(s => s.Value).Take(3).ToList())
            {
                product.Add(new ChartBestSellerProductViewModel
                {
                    ProductName = res.Where(o => o.ProductId == item.Key).FirstOrDefault().Product.Title,
                    SalesNumber = item.Value
                });
            }
            product.Add(new ChartBestSellerProductViewModel
            {
                ProductName = "بقیه محصولات",
                SalesNumber = productIdWithNumberSales.OrderByDescending(s => s.Value).Skip(3).Sum(s => s.Value)
            });

            return product;
        }

        public async Task<List<ChartBestSellerProductViewModel>> GetProductBestSellerCurrentMonthShamsi()
        {
            // get current month to shamsi and convert to miladi
            var dateTimeShamsi = DateTime.Now.ToShamsiDateTime();
            var dateTimeMiladi = new DateTime(dateTimeShamsi.Year, dateTimeShamsi.Month, 1).ToMiladi();

            var res = await _orderDetailRepository.GetOrderDetailsByDateTime(dateTimeMiladi);
            var duplicateProductId = res.Select(o => o.ProductId).Distinct().ToList();

            Dictionary<int, int> productIdWithNumberSales = new Dictionary<int, int>();
            foreach (var productId in duplicateProductId)
            {
                productIdWithNumberSales.Add(productId, res.Where(s => s.ProductId == productId).Sum(s => s.Count));
            }

            List<ChartBestSellerProductViewModel> product = new List<ChartBestSellerProductViewModel>();
            foreach (var item in productIdWithNumberSales.OrderByDescending(s => s.Value).Take(3).ToList())
            {
                product.Add(new ChartBestSellerProductViewModel
                {
                    ProductName = res.Where(o => o.ProductId == item.Key).FirstOrDefault().Product.Title,
                    SalesNumber = item.Value
                });
            }
            product.Add(new ChartBestSellerProductViewModel
            {
                ProductName = "بقیه محصولات",
                SalesNumber = productIdWithNumberSales.OrderByDescending(s => s.Value).Skip(3).Sum(s=> s.Value)
            });

            return product;
        }

        public async Task<List<OrderDetail>> GetDetailUserShoppingCartsByUserIdAndOrderId(int userId, int orderId)
        {
            return await _orderDetailRepository.GetDetailUserShoppingCartsByUserIdAndOrderId(userId, orderId);
        }

        #endregion
    }
}
