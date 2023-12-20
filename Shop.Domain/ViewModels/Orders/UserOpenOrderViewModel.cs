namespace Shop.Domain.ViewModels.Orders
{
    public class UserOpenOrderViewModel
    {
        public int UserId { get; set; }

        public List<UserOpenOrderDetailItemViewModel> Details { get; set; } = new List<UserOpenOrderDetailItemViewModel>();

        public int GetTotalPrice()
        {
            return Details.Sum(s => s.ProductPrice * s.Count);
        }

        public int GetTotalDiscounts()
        {
            return Details.Sum(s => s.GetOrderDetailWithDiscountPriceAmount());
        }
    }
}
