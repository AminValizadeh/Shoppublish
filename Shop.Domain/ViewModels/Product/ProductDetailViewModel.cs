using Shop.Domain.Models.Product;

namespace Shop.Domain.ViewModels.Product
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public string ImageName { get; set; }
        public int ProductPrice { get; set; }
        public List<string> Gallery { get; set; }
        public List<Models.Product.ProductColor> ProductColors { get; set; }
    }
}
