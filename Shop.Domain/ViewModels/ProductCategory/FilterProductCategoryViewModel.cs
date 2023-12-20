using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Product;

namespace Shop.Domain.ViewModels.ProductCategory
{
    public class FilterProductCategoryViewModel : BasePaging
    {
        #region properties

        public string? Title { get; set; }
        public List<Models.Product.ProductCategory> ProductCategories { get; set; }

        #endregion

        #region methods

        public FilterProductCategoryViewModel SetProductCategory(List<Models.Product.ProductCategory> productCategories)
        {
            this.ProductCategories = productCategories;
            return this;
        }

        public FilterProductCategoryViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StarPage = paging.StarPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
    }
}
