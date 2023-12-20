using Shop.Domain.Models.Account;
using Shop.Domain.Models.Common;

namespace Shop.Domain.Models.Permissions
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        #region Relations

        public User User { get; set; }
        public Role Role { get; set; }

        #endregion
    }
}
