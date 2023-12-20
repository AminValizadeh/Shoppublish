using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.ProductCategory;

namespace Shop.Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AddProductCategory(ProductCategory productCategory);
        Task UpdateProductCategory(ProductCategory productCategory);
        Task SaveChange();
        Task<ProductCategory> GetProductCategoryById(int id);
        Task<List<ProductCategory>> GetAllProductCategory();
        Task<FilterProductCategoryViewModel> FilterProductCategory(FilterProductCategoryViewModel filter);
    }
}
