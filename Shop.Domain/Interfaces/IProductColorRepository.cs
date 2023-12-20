using Shop.Domain.Models.Product;

namespace Shop.Domain.Interfaces
{
    public interface IProductColorRepository
    {
        Task<List<ProductColor>> GetAllProductColorBtProductId(int productId);
        Task<ProductColor> GetProductColorById(int id);
        Task AddProductColor(ProductColor productColor);
        Task UpdateProductColor(ProductColor productColor);
        Task SaveChange();
        Task<bool> IsExistProductColorByColorNameAndProductId(string colorName, int productId);
        Task<ProductColor> GetProductColorByColorNameAndProductId(string colorName, int productId);
    }
}
