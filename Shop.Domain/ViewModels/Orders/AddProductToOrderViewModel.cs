namespace Shop.Domain.ViewModels.Orders
{
    public class AddProductToOrderViewModel
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string? DiscountCode { get; set; }
    }
}
