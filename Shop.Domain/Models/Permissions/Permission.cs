using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Domain.Models.Permissions
{
    public class Permission : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string PermissionTitle { get; set; }

        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public Permission? Parent { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

        #endregion

    }
}
