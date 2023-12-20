using Shop.Domain.Models.Permissions;
using Shop.Domain.ViewModels.RolePermission;

namespace Shop.Application.Interfaces
{
    public interface IRolePermissionService
    {
        #region permission

        Task<List<Permission>> GetAllPermission();
        Task<List<int>> SelectedPermissionIDsByRoleId(int roleId);
        Task<bool> CheckPermission(int permissionId, int userId);
        Task AddPermissionsToRole(int roleId, List<int>? permissionId);
        Task UpdatePermissionRole(int roleId, List<int>? permissionId);

        #endregion

        #region role

        Task<List<Role>> GetRoles();
        Task<EditRoleViewModel?> GetRoleByIdForShowEdit(int roleId);
        Task AddRoleAndPermissions(AddRoleViewModel addRole);
        Task<string> GetRoleTitlebyId(int roleId);
        Task<EditRoleResult> UpdateRoleAndPermission(EditRoleViewModel editRole, int roleId);
        Task<bool> DeleteRole(int roleId);
        Task AddRolesToUser(List<int>? roles, int userId);
        Task RemoveRolesFromUser(int userId);
        Task EditRolesUser(List<int>? selectedRoleId, int userId);
        Task<List<int>> SelectedRoleIDsByUserId(int userId);

        #endregion
    }
}
