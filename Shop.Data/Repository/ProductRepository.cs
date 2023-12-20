using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Product;

namespace Shop.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public ProductRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddProduct(Product product)
        {
            product.CreateDate = DateTime.Now;
            await _context.Products.AddAsync(product);
            await SaveChange();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await SaveChange();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(s=> s.ProductGalleries).Include(s=> s.ProductColors).Where(s=> s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filter)
        {
            var query = _context.Products.Include(s => s.ProductCategory).Include(s=> s.ProductColors).AsQueryable();

            var expensiveProduct = await query.OrderByDescending(s => s.Price).FirstOrDefaultAsync();
            filter.FilterMaxPrice = expensiveProduct.Price;

            #region filter

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            if (filter.SelectedMaxPrice == 0) filter.SelectedMaxPrice = expensiveProduct.Price;

            query = query.Where(s => s.Price >= filter.SelectedMinPrice);
            query = query.Where(s => s.Price <= filter.SelectedMaxPrice);

            if (filter.CategoryId != null && filter.CategoryId != 0)
                query = query.Where(s => s.ProductCategory.Id == filter.CategoryId);

            #endregion

            #region paging

            var productsCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, productsCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProduct(allEntities);
        }

        public async Task<string> GetProductTitleByProductId(int productId)
        {
            return await _context.Products.Where(s => s.Id == productId).Select(s => s.Title).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistProductById(int productId)
        {
            return await _context.Products.AnyAsync(s => s.Id == productId);
        }

        public async Task<List<Product>> FilterProductByTitle(string title)
        {
            return await _context.Products
                .Where(u => EF.Functions.Like(u.Title, $"%{title}%")).ToListAsync();
        }

        public async Task<List<Product>> GetProducts(int take)
        {
            return await _context.Products.Include(p=> p.ProductColors).Take(take).OrderByDescending(s => s.CreateDate).ToListAsync();
        }
    }
}
