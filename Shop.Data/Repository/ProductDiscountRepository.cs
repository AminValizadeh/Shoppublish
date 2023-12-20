using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.ProductDiscount;

namespace Shop.Data.Repository
{
    public class ProductDiscountRepository : IProductDiscountRepository
    {

        #region constructor

        private readonly ShopContext _context;

        public ProductDiscountRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddProductDiscount(ProductDiscount productDiscount)
        {
            productDiscount.CreateDate = DateTime.Now;
            await _context.ProductDiscounts.AddAsync(productDiscount);
            await SaveChange();
        }

        public async Task<List<ProductDiscount>> GetAllProductDiscount()
        {
            return await _context.ProductDiscounts.ToListAsync();
        }

        public async Task<ProductDiscount> GetProductDiscountById(int id)
        {
            return await _context.ProductDiscounts.FindAsync(id);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductDiscount(ProductDiscount productDiscount)
        {
            _context.Update(productDiscount);
            await SaveChange();
        }

        public async Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filter)
        {
            var query = _context.ProductDiscounts.AsQueryable();

            #region filter

            if (!string.IsNullOrEmpty(filter.DiscountCode))
                query = query.Where(s => EF.Functions.Like(s.DiscountCode, $"%{filter.DiscountCode}%"));

            #endregion

            #region paging

            var productCategoriesCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, productCategoriesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProductDiscount(allEntities);
        }

        public async Task<ProductDiscount?> GetProductDiscountByDiscountCode(string discountCode)
        {
            return await _context.ProductDiscounts.FirstOrDefaultAsync(s=> s.DiscountCode == discountCode);
        }

        public async Task<ProductDiscount?> GetProductDiscountByDiscountCodeAndProductId(string discountCode, int productId)
        {
            return await _context.ProductDiscounts
                .FirstOrDefaultAsync(s=> s.DiscountCode == discountCode && s.ProductId == productId && !s.IsDelete);
        }

        public async Task<int?> GetPercentageDiscountByDiscountCodeAndProductId(string discountCode, int productId)
        {
            return await _context.ProductDiscounts.Where(s=> s.DiscountCode == discountCode && s.ProductId == productId && !s.IsDelete)
                .Select(s=> s.Percentage).FirstOrDefaultAsync();
        }
    }
}
