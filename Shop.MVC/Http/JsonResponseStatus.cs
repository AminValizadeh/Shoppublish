using Microsoft.AspNetCore.Mvc;

namespace Shop.MVC.Http
{
    public static class JsonResponseStatus
    {
        public static JsonResult SendStatus(JsonResponseStatusType type,string message, object? data) =>
            new JsonResult(new { status = type.ToString(), message, data });

    }
    public enum JsonResponseStatusType
    {
        Success,
        Warning,
        Danger,
        Info
    }

}
