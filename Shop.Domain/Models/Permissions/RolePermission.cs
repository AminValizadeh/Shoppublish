using Shop.Domain.Models.Common;

namespace Shop.Domain.Models.Permissions
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        #region Relations

        public Role Role { get; set; }
        public Permission Permission { get; set; }

        #endregion
    }
}
