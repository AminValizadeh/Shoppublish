using Shop.Domain.Models.Permissions;

namespace Shop.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<List<Permission>> GetAllPermissions();
        Task<List<int>> SelectedPermissionIDsByRoleId(int roleId);
        Task<List<int>> SelectedRoleIDsByPermissionId(int permissionId);
        Task AddPermissionsToRole(List<RolePermission> rolePermissions);
        Task<List<RolePermission>> GetRolePermissionsByRoleId(int roleId);
        Task RemoveRolePermissions(List<RolePermission> rolePermissions);
        Task SaveChange();
    }
}
