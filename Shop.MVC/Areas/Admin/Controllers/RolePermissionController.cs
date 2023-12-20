using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.Models.Permissions;
using Shop.Domain.ViewModels.RolePermission;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/role-permission/")]
    public class RolePermissionController : AdminBaseController
    {

        #region constructor

        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        #endregion


        #region list role

        [HttpGet("role-permission-list")]
        public async Task<IActionResult> Index()
        {
            List<Role> roles = await _rolePermissionService.GetRoles();
            return View(roles);
        }

        #endregion

        #region create role

        [HttpGet("role-permission-create")]
        public async Task<IActionResult> CreateRolePermission()
        {
            ViewData["Permissions"] = await _rolePermissionService.GetAllPermission();
            return View();
        }


        [HttpPost("role-permission-create"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRolePermission(AddRoleViewModel addRole)
        {

            if (ModelState.IsValid)
            {
                 await _rolePermissionService.AddRoleAndPermissions(addRole);
                TempData[SuccessMessage] = "نفش با موفقیت اضافه شد";
                return RedirectToAction("Index");   
            }

            ViewData["Permissions"] = await _rolePermissionService.GetAllPermission();
            return View(addRole);
        }

        #endregion

        #region edit role

        [HttpGet("role-permission-edit/{roleId}")]
        public async Task<IActionResult> EditRolePermission(int roleId)
        {
           var role = await _rolePermissionService.GetRoleByIdForShowEdit(roleId);

            if(role == null)
            {
                TempData[ErrorMessage] = "نقشی پیدا نشد";
                return RedirectToAction("Index");
            }

            ViewData["Permissions"] = await _rolePermissionService.GetAllPermission();
            return View(role);
        }

        [HttpPost("role-permission-edit/{roleId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRolePermission(EditRoleViewModel editRole,int roleId)
        {
            if (ModelState.IsValid)
            {
                var res = await _rolePermissionService.UpdateRoleAndPermission(editRole,roleId);
                switch (res)
                {
                    case EditRoleResult.Success:
                        TempData[SuccessMessage] = "نقش با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                    case EditRoleResult.NotFoundRole:
                        TempData[SuccessMessage] = "در ویرایش نقش مشکلی وجود دارد";
                        break;
                }
            }

            ViewData["Permissions"] = await _rolePermissionService.GetAllPermission();
            return View(editRole);
        }

        #endregion

        #region delete role

        [HttpGet("role-permission-delete/{roleId}")]
        public async Task<IActionResult> DeleteRolePermission(int roleId)
        {
            var role = await _rolePermissionService.DeleteRole(roleId);

            if (role)
                TempData[SuccessMessage] = "نقش با موفقیت حذف شد";
            else
                TempData[SuccessMessage] = "در حذف نقش مشکلی به وجود آمد";
            
            return RedirectToAction("Index");
        }

        #endregion
    }
}
