using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.ProductCategory;

namespace Shop.Domain.ViewModels.ProductDiscount
{
    public class FilterProductDiscountViewModel : BasePaging
    {
        #region properties

        public string? DiscountCode { get; set; }
        public List<Models.Product.ProductDiscount> ProductDiscounts { get; set; }

        #endregion

        #region methods

        public FilterProductDiscountViewModel SetProductDiscount(List<Models.Product.ProductDiscount> productDiscounts)
        {
            this.ProductDiscounts = productDiscounts;
            return this;
        }

        public FilterProductDiscountViewModel SetPaging(BasePaging paging)
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
