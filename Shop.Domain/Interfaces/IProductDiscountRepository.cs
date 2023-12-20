using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.ProductDiscount;

namespace Shop.Domain.Interfaces
{
    public interface IProductDiscountRepository
    {
        Task AddProductDiscount(ProductDiscount productDiscount);
        Task UpdateProductDiscount(ProductDiscount productDiscount);
        Task SaveChange();
        Task<ProductDiscount> GetProductDiscountById(int id);
        Task<List<ProductDiscount>> GetAllProductDiscount();
        Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filter);
        Task<ProductDiscount?> GetProductDiscountByDiscountCode(string discountCode);
        Task<ProductDiscount?> GetProductDiscountByDiscountCodeAndProductId(string discountCode, int productId);
        Task<int?> GetPercentageDiscountByDiscountCodeAndProductId(string discountCode, int productId);
    }
}
