using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Permissions;
using Shop.Domain.ViewModels.RolePermission;

namespace Shop.Application.Services
{
    public class RolePermissionService : IRolePermissionService
    {

        #region constructor

        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;

        public RolePermissionService(IPermissionRepository permissionRepository, IRoleRepository roleRepository)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
        }

        #endregion

        #region permission

        public async Task<List<Permission>> GetAllPermission()
        {
            return await _permissionRepository.GetAllPermissions();
        }

        public async Task<List<int>> SelectedPermissionIDsByRoleId(int roleId)
        {
            return await _permissionRepository.SelectedPermissionIDsByRoleId(roleId);
        }

        public async Task<bool> CheckPermission(int permissionId, int userId)
        {
            List<int> roleIDsUser = await _roleRepository.SelectedRoleIDsByUserId(userId);

            if (!roleIDsUser.Any())
                return false;

            List<int> roleIDsPermission = await _permissionRepository.SelectedRoleIDsByPermissionId(permissionId);

            return roleIDsPermission.Any(p => roleIDsUser.Contains(p));
        }

        public async Task AddPermissionsToRole(int roleId, List<int>? permissionId)
        {
            if (permissionId != null)
            {
                List<RolePermission> rolePermissions = new List<RolePermission>();

                foreach (var id in permissionId)
                {
                    rolePermissions.Add(new RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = id,
                        CreateDate = DateTime.Now
                    });
                }
                await _permissionRepository.AddPermissionsToRole(rolePermissions);
            }
        }

        public async Task UpdatePermissionRole(int roleId, List<int>? permissionId)
        {
            var rolePermission = await _permissionRepository.GetRolePermissionsByRoleId(roleId);
            await _permissionRepository.RemoveRolePermissions(rolePermission);
            await AddPermissionsToRole(roleId, permissionId);
        }

        #endregion

        #region role

        public async Task<List<Role>> GetRoles()
        {
            return await _roleRepository.GetRoles();
        }

        public async Task<EditRoleViewModel?> GetRoleByIdForShowEdit(int roleId)
        {
            Role role = await _roleRepository.GetRoleById(roleId);

            if (role == null) return null;

            return new EditRoleViewModel()
            {
                RoleTitle = role.RoleTitle,
                SelectedPermission = await SelectedPermissionIDsByRoleId(roleId)
            };
        }

        public async Task AddRoleAndPermissions(AddRoleViewModel addRole)
        {
            Role role = new Role()
            {
                RoleTitle = addRole.RoleTitle
            };
            int roleId = await _roleRepository.AddRole(role);


            await AddPermissionsToRole(roleId, addRole.SelectedPermission);

        }

        public async Task<string> GetRoleTitlebyId(int roleId)
        {
            return await _roleRepository.GetRoleTitlebyId(roleId);
        }

        public async Task<EditRoleResult> UpdateRoleAndPermission(EditRoleViewModel editRole, int roleId)
        {
            var role = await _roleRepository.GetRoleById(roleId);

            if (role == null) return EditRoleResult.NotFoundRole;

            role.RoleTitle = editRole.RoleTitle;

            await _roleRepository.UpdateRole(role);

            await UpdatePermissionRole(roleId, editRole.SelectedPermission);


            return EditRoleResult.Success;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            Role role = await _roleRepository.GetRoleById(roleId);
            if (role == null) return false;
            role.IsDelete = true;
            await _roleRepository.UpdateRole(role);
            return true;
        }

        public async Task AddRolesToUser(List<int>? roles, int userId)
        {
           if(roles != null)
            {
                List<UserRole> userRoles = new List<UserRole>();
                foreach (var role in roles)
                {
                    userRoles.Add(new UserRole()
                    {
                        CreateDate = DateTime.Now,
                        RoleId = role,
                        UserId = userId
                    });
                }
                await _roleRepository.AddRolesToUser(userRoles);
            }
        }

        public async Task RemoveRolesFromUser(int userId)
        {
            var userRoles = await _roleRepository.GetUserRolesByUserId(userId);

            await _roleRepository.RemoveUserRoles(userRoles);
        }

        public async Task EditRolesUser(List<int>? selectedRoleId, int userId)
        {
            await RemoveRolesFromUser(userId);

            if(selectedRoleId != null)
            {
                List<UserRole> userRoles = new List<UserRole>();
                foreach (var roleId in selectedRoleId)
                {
                    userRoles.Add(new UserRole()
                    {
                        RoleId = roleId,
                        UserId = userId,
                        CreateDate = DateTime.Now
                    });
                }

                await _roleRepository.AddRolesToUser(userRoles);
            }
        }

        public async Task<List<int>> SelectedRoleIDsByUserId(int userId)
        {
            return await _roleRepository.SelectedRoleIDsByUserId(userId);
        }

        #endregion
    }
}
