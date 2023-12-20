using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;

namespace Shop.Data.Repository
{
    public class ProductColorRepository : IProductColorRepository
    {

        #region constructor

        private readonly ShopContext _context;

        public ProductColorRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion
        public async Task AddProductColor(ProductColor productColor)
        {
            productColor.CreateDate = DateTime.Now;
            await _context.ProductColors.AddAsync(productColor);
            await SaveChange();
        }

        public async Task<List<ProductColor>> GetAllProductColorBtProductId(int productId)
        {
            return await _context.ProductColors.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<ProductColor> GetProductColorByColorNameAndProductId(string colorName, int productId)
        {
            return await _context.ProductColors.FirstOrDefaultAsync(p=> p.ColorName == colorName && p.ProductId == productId && !p.IsDelete);
        }

        public async Task<ProductColor> GetProductColorById(int id)
        {
            return await _context.ProductColors.FindAsync(id);
        }

        public async Task<bool> IsExistProductColorByColorNameAndProductId(string colorName, int productId)
        {
            return await _context.ProductColors.AnyAsync(p=> p.ColorName == colorName && p.ProductId == productId && !p.IsDelete);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductColor(ProductColor productColor)
        {
            _context.ProductColors.Update(productColor);
            await SaveChange();
        }
    }
}
