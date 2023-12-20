
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Application.Extensions;
using Shop.Application.Utils;

namespace Shop.MVC.Controllers
{
    public class UploaderController : Controller
    {

        [HttpPost]
        public IActionResult? UploadImage(IFormFile upload, string CKEditorFuncName, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            if (!upload.IsImage())
            {
                var notImageMessage = "";
                var notImage = JsonConvert.DeserializeObject("{'uploaded':0, 'error': {'message': \" " + notImageMessage + " \"}}");
                return Json(notImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            upload.AddImageToServer(fileName, PathExtension.UploadImageTicketServer, null, null, null, null);

            return Json(new
            {
                uploaded= true,
                url = $"{PathExtension.UploadImageTicket}{fileName}"
            });
        }
    }
}
