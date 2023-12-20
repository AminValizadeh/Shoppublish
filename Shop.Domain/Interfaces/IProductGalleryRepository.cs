using Shop.Domain.Models.Product;

namespace Shop.Domain.Interfaces
{
    public interface IProductGalleryRepository
    {
        Task AddProductGallery(ProductGallery productGallery);
        Task UpdateProductGallery(ProductGallery productGallery);
        Task<ProductGallery> GetProductGalleryById(int id);
        Task<List<ProductGallery>> GetProductGalleryByProductId(int productId);
        Task SaveChange();
    }
}
