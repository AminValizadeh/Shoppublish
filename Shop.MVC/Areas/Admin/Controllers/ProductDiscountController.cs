using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Application.Services;
using Shop.Domain.ViewModels.ProductDiscount;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/product-discount/")]
    public class ProductDiscountController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductDiscountController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region list discount

        [HttpGet("product-discount-list")]
        public async Task<IActionResult> Index(FilterProductDiscountViewModel filter)
        {
            filter = await _productService.FilterProductDiscountInAdmin(filter);
            return View(filter);
        }

        #endregion

        #region create discount

        [HttpGet("product-discount-create")]
        public IActionResult CreateProductDiscount()
        {
            return View();
        }

        [HttpPost("product-discount-create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductDiscount(CreateProductDiscountViewModel createDiscount)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.CreateProductDiscountInAdmin(createDiscount);
                switch (res)
                {
                    case CreateProductDiscountResult.Success:
                        TempData[SuccessMessage] = "کد تخفیف با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                    case CreateProductDiscountResult.ExistDiscount:
                        TempData[ErrorMessage] = "یک کد تخفیف با همین کد وجود دارد، لطفا کد تغییر دهید";
                        break;
                }
            }
            return View(createDiscount);
        }

        #endregion

        #region get product json

        [HttpGet("product-autocomplete")]
        public async Task<IActionResult> GetSellerProductsJson(string title)
        {
            var data = await _productService.FilterProducstByTitle(title);
            return new JsonResult(data);
        }

        #endregion

        #region edit discount

        [HttpGet("product-discount-edit/{productDiscountId}")]
        public async Task<IActionResult> EditProductDiscount(int productDiscountId)
        {
            var res = await _productService.GetProductDiscountForEditInAdmin(productDiscountId);
            if (res == null)
            {
                TempData[ErrorMessage] = "کد تخفیف مورد نظر یافت نشد";
                return RedirectToAction("Index");
            }
            return View(res);
        }

        [HttpPost("product-discount-edit/{productDiscountId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductDiscount(UpdateProductDiscountViewModel productDiscount, int productDiscountId)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditProductDiscountInAdmin(productDiscount, productDiscountId);

                switch (res)
                {
                    case UpdateProductDiscountResult.Success:
                        TempData[SuccessMessage] = "کد تخفیف با موفقیت ویرایش شد";
                        return RedirectToAction("Index");
                    case UpdateProductDiscountResult.ExistDiscount:
                        TempData[ErrorMessage] = "یک کد تخفیف با همین کد وجود دارد، لطفا کد تغییر دهید";
                        break;
                    case UpdateProductDiscountResult.NotFoundDiscount:
                        TempData[ErrorMessage] = "کد تخفیف برای ویرایش پیدا نشد";
                        break;
                    
                }
            }
            return View(productDiscount);
        }

        #endregion

        #region delete discount

        [HttpGet("product-discount-delete/{productDiscountId}")]
        public async Task<IActionResult> DeleteProductDiscount(int productDiscountId)
        {
            var res = await _productService.DeleteProductDiscountInAdmin(productDiscountId);
            if (res)
                TempData[SuccessMessage] = "کد تخفیف با موفقیت حذف شد";
            else
                TempData[ErrorMessage] = "مشکلی در حذف کد تخفیف به وجود امد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
