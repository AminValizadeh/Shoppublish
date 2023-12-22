using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Ticket;

namespace Shop.Domain.ViewModels.Product
{
    public class FilterProductViewModel : BasePaging
    {

        #region properties

        public string? Title { get; set; }
        public int? CategoryId { get; set; }

        public int FilterMinPrice { get; set; }
        public int FilterMaxPrice { get; set; }
        public int SelectedMinPrice { get; set; }
        public int SelectedMaxPrice { get; set; }
        public int PriceStep { get; set; } = 50000;

        public List<Models.Product.Product> Products { get; set; }

        #endregion

        #region methods

        public FilterProductViewModel SetProduct(List<Models.Product.Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductViewModel SetPaging(BasePaging paging)
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

        public enum filterresult
        { 
        NotFound
        }
    }
}
