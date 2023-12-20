using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;

namespace Shop.Data.Repository
{
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public ProductGalleryRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddProductGallery(ProductGallery productGallery)
        {
            productGallery.CreateDate = DateTime.Now;
            await _context.ProductGalleries.AddAsync(productGallery);
            await SaveChange();
        }

        public async Task<List<ProductGallery>> GetProductGalleryByProductId(int productId)
        {
            return await _context.ProductGalleries.Where(s=> s.ProductId == productId).ToListAsync();
        }

        public async Task<ProductGallery> GetProductGalleryById(int id)
        {
            return await _context.ProductGalleries.FindAsync(id);
        }

        public async Task SaveChange()
        {
           await _context.SaveChangesAsync();
        }

        public async Task UpdateProductGallery(ProductGallery productGallery)
        {
            _context.ProductGalleries.Update(productGallery);
            await SaveChange();
        }
    }
}
