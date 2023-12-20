using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Product;

namespace Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task SaveChange();
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetAllProduct();
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter);
        Task<string> GetProductTitleByProductId(int productId);
        Task<bool> IsExistProductById(int productId);
        Task<List<Product>> FilterProductByTitle(string title);
        Task<List<Product>> GetProducts(int take);
    }
}
