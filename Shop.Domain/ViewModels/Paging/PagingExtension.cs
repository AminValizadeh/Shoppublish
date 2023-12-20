using System.Linq;

namespace Shop.Domain.ViewModels.Paging
{
    public static class PagingExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging paging) =>
            query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
    }
}
