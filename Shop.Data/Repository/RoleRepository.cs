using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Permissions;

namespace Shop.Data.Repository
{
    public class RoleRepository : IRoleRepository
    {
        #region constructor

        private readonly ShopContext _context;

        public RoleRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<List<int>> SelectedRoleIDsByUserId(int userId)
        {
            return await _context.UserRoles.Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId).ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<int> AddRole(Role role)
        {
            role.CreateDate = DateTime.Now;
            await _context.Roles.AddAsync(role);
            await SaveChange();
            return role.Id;
        }

        public async Task<string> GetRoleTitlebyId(int roleId)
        {
            return await _context.Roles.Where(r => r.Id == roleId).Select(r => r.RoleTitle).FirstOrDefaultAsync();
        }

        public async Task AddRolesToUser(List<UserRole> userRoles)
        {
            await _context.UserRoles.AddRangeAsync(userRoles);
            await SaveChange();
        }

        public async Task<List<UserRole>> GetUserRolesByUserId(int userId)
        {
            return await _context.UserRoles.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task RemoveUserRoles(List<UserRole> userRoles)
        {
             _context.UserRoles.RemoveRange(userRoles);
            await SaveChange();
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            await SaveChange();
        }
    }
}
