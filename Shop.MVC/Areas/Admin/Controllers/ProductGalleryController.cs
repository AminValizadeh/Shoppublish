using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.ProductGallery;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/product-gallery/")]
    public class ProductGalleryController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductGalleryController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region list product gallery

        [HttpGet("product-gallery-list/{productId}")]
        public async Task<IActionResult> Index(int productId)
        {
            var productGallery = await _productService.GetProductGalleryByProductId(productId);
            return View(productGallery);
        }

        #endregion

        #region create product gallery

        [HttpGet("product-gallery-create/{productId}")]
        public async Task<IActionResult> CreateProductGallery(int productId)
        {
            var productGallery = new CreateProductGalleryViewModel
            {
                ProductTitle = await _productService.GetProductTitleByProductId(productId),
                ProductId = productId
            };
            return View(productGallery);
        }

        [HttpPost("product-gallery-create/{productId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductGallery(CreateProductGalleryViewModel createProductGallery)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductGallery(createProductGallery);
                switch (res)
                {
                    case CreateProductGalleryResult.Success:
                        TempData[SuccessMessage] = "تصویر مورد نظر با موفقیت به گالری محصولات اضافه شد";
                        return RedirectToAction("Index", new { productId = createProductGallery.ProductId });
                    case CreateProductGalleryResult.NotFoundProduct:
                        TempData[WarningMessage] = "محصول مورد نظر پیدا نشد";
                        break;
                    case CreateProductGalleryResult.NotValidImage:
                        TempData[ErrorMessage] = "تصویر اپلود شده مشکل دارد";
                        break;
                }
            }
            return View(createProductGallery);
        }

        #endregion

        #region edit product gallery

        [HttpGet("product-gallery-edit/{productGalleryId}")]
        public async Task<IActionResult> EditProductGallery(int productGalleryId)
        {
            var productGallery = await _productService.GetProductGalleryByIdForEdit(productGalleryId);

            if(productGallery == null)
            {
                TempData[ErrorMessage] = "تصویری در گالری پیدا نشد";
                return Redirect("/admin/product/product-list");
            }

            return View(productGallery);
        }

        [HttpPost("product-gallery-edit/{productGalleryId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductGallery(int productGalleryId, UpdateProductGalleryViewModel updateProductGallery)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProductGallery(productGalleryId,updateProductGallery);

                switch (res)
                {
                    case UpdateProductGalleryResult.Success:
                        TempData[SuccessMessage] = "تصویر مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("Index", new { productId = updateProductGallery.ProductId });
                    case UpdateProductGalleryResult.NotFoundProductGallery:
                        TempData[WarningMessage] = "محصول مورد نظر پیدا نشد";
                        break;
                    case UpdateProductGalleryResult.NotValidImage:
                        TempData[ErrorMessage] = "تصویر اپلود شده مشکل دارد";
                        break;
                }
            }

            return View(updateProductGallery);
        }

        #endregion

        #region delete product gallery

        [HttpGet("product-gallery-delete/{productGalleryId}/{productId}")]
        public async Task<IActionResult> DeleteProductGallery(int productGalleryId, int productId)
        {
            var res = await _productService.DeleteProductGalleryById(productGalleryId);
            if (res)
                TempData[SuccessMessage] = "تصویر از گالری با موفقیت حذف شد";
            else
                TempData[SuccessMessage] = "در حذف تصویر از گالری مشکلی پیش امد";

            return RedirectToAction("Index", new { productId = productId });
        }

        #endregion
    }
}
