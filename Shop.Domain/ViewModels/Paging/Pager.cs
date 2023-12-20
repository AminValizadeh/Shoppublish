namespace Shop.Domain.ViewModels.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId, int allEntitiesCount, int take, int HowManyShowPageAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));
            return new BasePaging
            {
                PageId = pageId,
                AllEntitiesCount = allEntitiesCount,
                TakeEntity = take,
                SkipEntity = (pageId - 1) * take,
                StarPage = pageId - HowManyShowPageAfterAndBefore <= 0 ? 1 : pageId - HowManyShowPageAfterAndBefore,
                EndPage = pageId + HowManyShowPageAfterAndBefore > pageCount ? pageCount : pageId + HowManyShowPageAfterAndBefore,
                HowManyShowPageAfterAndBefore = HowManyShowPageAfterAndBefore,
                PageCount = pageCount,
            };
        }
    }
}
