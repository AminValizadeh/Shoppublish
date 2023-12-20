using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shop.Domain.Models.Permissions
{
    public class Role : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string RoleTitle { get; set; }

        #region Relations

        public List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
