using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Permissions;

namespace Shop.Data.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public PermissionRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Permission>> GetAllPermissions()
        {
            return await _context.Permissions.ToListAsync();
        }

        public async Task<List<int>> SelectedPermissionIDsByRoleId(int roleId)
        {
            return await _context.RolePermissions.Where(r => r.RoleId == roleId).Select(r => r.PermissionId).ToListAsync();
        }

        public async Task<List<int>> SelectedRoleIDsByPermissionId(int permissionId)
        {
            return await _context.RolePermissions.Where(p => p.PermissionId == permissionId).Select(p => p.RoleId).ToListAsync();
        }

        public async Task AddPermissionsToRole(List<RolePermission> rolePermissions)
        {
            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await SaveChange();
        }

        public async Task<List<RolePermission>> GetRolePermissionsByRoleId(int roleId)
        {
            return await _context.RolePermissions.Where(r => r.RoleId == roleId).ToListAsync();
        }

        public async Task RemoveRolePermissions(List<RolePermission> rolePermissions)
        {
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

    }
}
