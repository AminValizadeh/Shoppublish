namespace Shop.Domain.ViewModels.Product
{
    public class ProductCardViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ImageName { get; set; }
        public int ProductPrice { get; set; }
        public List<string> ColorCodes { get; set; }
    }
}
