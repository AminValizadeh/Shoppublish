using System.Security.Claims;
using System.Security.Principal;

namespace Shop.MVC.PresentationExtensions
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this ClaimsPrincipal? claimsPrincipal)
        {
            if (claimsPrincipal != null)
            {
                // var data1 = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)?.ToString();
                var data = claimsPrincipal.Claims.SingleOrDefault(s=> s.Type == ClaimTypes.NameIdentifier);
                if (data != null) return int.Parse(data.Value);
            }
            return default;
        }

        public static int GetUserId(this IPrincipal principal)
        {
            var user = principal as ClaimsPrincipal;
            return user.GetUserId();
        }
    }
}
