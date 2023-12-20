using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.ProductCategory;

namespace Shop.Data.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public ProductCategoryRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddProductCategory(ProductCategory productCategory)
        {
            productCategory.CreateDate = DateTime.Now;
            await _context.ProductCategories.AddAsync(productCategory);
            await SaveChange();
        }

        public async Task UpdateProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Update(productCategory);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetAllProductCategory()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetProductCategoryById(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<FilterProductCategoryViewModel> FilterProductCategory(FilterProductCategoryViewModel filter)
        {
            var query = _context.ProductCategories.AsQueryable();

            #region filter

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.Title}%"));

            #endregion

            #region paging

            var productCategoriesCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, productCategoriesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProductCategory(allEntities);
        }
    }
}
