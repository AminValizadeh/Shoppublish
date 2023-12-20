namespace Shop.MVC.PresentationExtensions
{
    public static class HttpExtensions
    {
        public static string GetUserIp(this HttpContext httpContext) =>
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "";
    }
}
