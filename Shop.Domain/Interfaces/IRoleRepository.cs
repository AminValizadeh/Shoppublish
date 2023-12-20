using Shop.Domain.Models.Permissions;

namespace Shop.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();
        Task<List<int>> SelectedRoleIDsByUserId(int userId);
        Task<Role> GetRoleById(int roleId);
        Task<int> AddRole(Role role);
        Task<string> GetRoleTitlebyId(int roleId);
        Task AddRolesToUser(List<UserRole> userRoles);
        Task<List<UserRole>> GetUserRolesByUserId(int userId);
        Task RemoveUserRoles(List<UserRole> userRoles);
        Task SaveChange();
        Task UpdateRole(Role role);
    }
}
