using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Product;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/product/")]
    public class ProductController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region product list

        [HttpGet("product-list")]
        public async Task<IActionResult> Index(FilterProductViewModel filter)
        {
            filter = await _productService.FilterProducts(filter);
            return View(filter);
        }

        #endregion

        #region create product

        [HttpGet("product-create")]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _productService.GetAllProductCategories();
            ViewData["Categories"] = new SelectList(categories, "Value", "Text");
            return View();
        }

        [HttpPost("product-create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductInAdmin(product);

                switch (res)
                {
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = "محصول مورد نظر با موفقیت اضافه شد";
                        return RedirectToAction("Index");
                    case CreateProductResult.NotValidImage:
                        TempData[WarningMessage] = "فرمت تصویر وارد شده درست نمی باشد";
                        break;
                }
            }
            return View(product);
        }

        #endregion

        #region edit product

        [HttpGet("product-edit/{productId}")]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var editProduct = await _productService.GetProductByIdForEditInAdmin(productId);
            if(editProduct == null)
            {
                TempData[ErrorMessage] = "محصول مورد نظر پیدا نشد";
                return RedirectToAction("Index");
            }

            var categories = await _productService.GetAllProductCategories();
            ViewData["Categories"] = new SelectList(categories, "Value", "Text",new { Value = productId });

            return View(editProduct);
        }

        [HttpPost("product-edit/{productId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(UpdateProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProductinAdmin(product);
                switch (res)
                {
                    case UpdateProductResult.Success:
                        TempData[SuccessMessage] = "محصول مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                    case UpdateProductResult.NotValidImage:
                        TempData[SuccessMessage] = "تصویر اپلود شده مشکل دارد";
                        break;
                    case UpdateProductResult.NotFoundProduct:
                        TempData[SuccessMessage] = "محصول مورد نظر پیدا نشد";
                        break;
                }
            }
            return View(product);
        }

        #endregion

        #region delete product

        [HttpGet("product-delete/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var res =await _productService.DeleteProductByIdInAdmin(productId);
            if (res)
                TempData[SuccessMessage] = "محصول مورد نظر با موفقیت حذف شد";
            else
                TempData[SuccessMessage] = "خطایی در حذف محصول رخ داد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
