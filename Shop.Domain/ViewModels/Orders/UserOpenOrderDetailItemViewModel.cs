namespace Shop.Domain.ViewModels.Orders
{
    public class UserOpenOrderDetailItemViewModel
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductImageName { get; set; } = string.Empty;
        public int Count { get; set; }
        public int ProductPrice { get; set; }
        public int? DiscountPercentage { get; set; }
        public string? DiscountCode { get; set; }

        public int GetOrderDetailWithDiscountPriceAmount()
        {
            if (this.DiscountPercentage != null)
            {
                return ((this.ProductPrice * this.DiscountPercentage.Value) / 100) * this.Count;
            }

            return 0;
        }

        public string GetOrderDetailWithDiscountPrice()
        {
            if (this.DiscountPercentage != null)
            {
                return this.GetOrderDetailWithDiscountPriceAmount().ToString("#,0");
            }

            return "------";
        }

        public int GetTotalAmountByDiscount()
        {
            return (int)(this.ProductPrice * this.Count) - this.GetOrderDetailWithDiscountPriceAmount();
        }
    }
}
