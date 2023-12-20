using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.ProductColor;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/product-color/")]
    public class ProductColorController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductColorController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region list product color

        [HttpGet("product-color-list/{productId}")]
        public async Task<IActionResult> Index(int productId)
        {
            var productColors = await _productService.GetProductColorsByProductId(productId);
            return View(productColors);
        }

        #endregion

        #region create product color

        [HttpGet("product-color-create/{productId}")]
        public async Task<IActionResult> CreateProductColor(int productId)
        {
            var res = await _productService.GetProductForAddProductColorToProduct(productId);
            if(res == null)
            {
                TempData[ErrorMessage] = "هیچ اطلاعاتی از محصول مورد نظر پیدا نشد";
                return RedirectToAction("Index", new { productId });
            }

            return View(res);
        }

        [HttpPost("product-color-create/{productId}")]
        public async Task<IActionResult> CreateProductColor(int productId,CreateProductColorViewModel createProductColor)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductColor(createProductColor);

                switch (res)
                {
                    case CreateProductColorResult.Success:
                        TempData[SuccessMessage] = "رنگ مورد نظر با موفقیت به محصول اضافه شد";
                        return RedirectToAction("Index", new { productId });
                    case CreateProductColorResult.ExistProductColorForProduct:
                        TempData[InfoMessage] = "رنگ مورد نظر برای این محصول وجود دارد";
                        break;
                    case CreateProductColorResult.NotFoundProduct:
                        TempData[ErrorMessage] = "محصول مورد نظر پیدا نشد";
                        break;
                }
            }
            return View(createProductColor);
        }

        #endregion

        #region edit product color

        [HttpGet("product-color-edit/{productColorId}")]
        public async Task<IActionResult> EditProductColor(int productColorId)
        {
            var res = await _productService.GetProductColorForEdit(productColorId);
            if (res == null)
            {
                TempData[ErrorMessage] = "رنگ محصول مورد نظر پیدا نشد";
                return Redirect("/admin/product/product-list");
            }
            return View(res);
        }

        [HttpPost("product-color-edit/{productColorId}")]
        public async Task<IActionResult> EditProductColor(int productColorId, UpdateProductColorViewModel updateProductColor)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.UpdateProductColor(updateProductColor, productColorId);
                switch (res)
                {
                    case UpdateProductColorResult.Success:
                        TempData[SuccessMessage] = "رنگ محصول با موفقیت ویرایش شد";
                        return RedirectToAction("Index", new { productId = updateProductColor.ProductId });
                    case UpdateProductColorResult.NotFoundProductColor:
                        TempData[ErrorMessage] = "رنگ محصول پیدا نشد";
                        break;
                    case UpdateProductColorResult.ExistProductColorForProduct:
                        TempData[InfoMessage] = "رنگ مورد نظر برای این محصول وجود دارد";
                        break;
                }
            }
            return View(updateProductColor);
        }

        #endregion

        #region delete product color

        [HttpGet("product-color-delete/{productColorId}/{productId}")]
        public async Task<IActionResult> DeleteProductColor(int productColorId, int productId)
        {
            var res = await _productService.DeleteProductColor(productColorId,productId);

            if (res)
                TempData[SuccessMessage] = "رنگ محصول با موفقیت حذف شد";
            else
                TempData[ErrorMessage] = "در حذف رنگ محصول مشکلی به وجود آمد";

            return RedirectToAction("Index", new { productId });
        }

        #endregion
    }
}
