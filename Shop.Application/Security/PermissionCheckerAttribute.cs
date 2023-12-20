using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Application.Interfaces;
using System.Security.Claims;

namespace Shop.Application.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IRolePermissionService _permissionService;
        private int _permissionId = 0;

        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permissionService = (IRolePermissionService)context.HttpContext.RequestServices
                .GetService(typeof(IRolePermissionService));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

                if (_permissionService.CheckPermission(_permissionId, userId).Result == false)
                {
                    context.Result = new RedirectResult("/login");
                    //context.Result = new RedirectResult("/login?"+context.HttpContext.Request.Path);
                }
            }
            else
            {
                context.Result = new RedirectResult("/login");
                //context.Result = new RedirectResult("/login?"+context.HttpContext.Request.Path);
            }
        }
    }
}
