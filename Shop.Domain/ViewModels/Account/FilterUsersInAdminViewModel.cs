using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Account
{
    public class FilterUsersInAdminViewModel : BasePaging
    {
        #region properties

        public string? Mobile { get; set; }
        public string? Email { get; set; }


        public List<User> Users { get; set; } = new List<User>();

        #endregion

        #region methods

        public FilterUsersInAdminViewModel SetUsers(List<User> user)
        {
            this.Users = user;
            return this;
        }

        public FilterUsersInAdminViewModel SetPaging(BasePaging paging)
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
