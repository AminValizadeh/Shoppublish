using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.ProductCategory;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/product-category/")]
    public class ProductCategoryController : AdminBaseController
    {

        #region constructor

        private readonly IProductService _productService;

        public ProductCategoryController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region list category

        [HttpGet("product-category-list")]
        public async Task<IActionResult> Index(FilterProductCategoryViewModel filter)
        {
            filter = await _productService.FilterProductCategoryInAdmin(filter);
            return View(filter);
        }

        #endregion

        #region create category

        [HttpGet("product-category-create/{parentId?}")]
        public async Task<IActionResult> CreateProductCategory(int? parentId)
        {
            if (parentId != null)
            {
                var getProductCategory = await _productService.GetProductCategoryById(parentId.Value);
                var productCategory = new CreateOrEditProductCategoryViewModel
                {
                    ParentTitle = getProductCategory.Title,
                    ParentId = parentId.Value,
                };
                return View(productCategory);
            }
            return View();
        }

        [HttpPost("product-category-create/{parentId?}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateOrEditProductCategoryViewModel createProductCategory, int? parentId)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProductCategoryInAdmin(createProductCategory);
                TempData[SuccessMessage] = "دسته بندی با موفقیت اضافه شد";
                return RedirectToAction("Index");
            }

            return View(createProductCategory);
        }

        #endregion

        #region edit category

        [HttpGet("product-category-edit/{productCategoryId}")]
        public async Task<IActionResult> EditProductCategory(int productCategoryId)
        {
            var category = await _productService.GetProductCategoryByIdForEditinAdmin(productCategoryId);
            if (category == null)
            {
                TempData[ErrorMessage] = "دسته بندی پیدا نشد";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost("product-category-edit/{productCategoryId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(int productCategoryId, CreateOrEditProductCategoryViewModel editProductCategory)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.UpdateProductCategoryInAdmin(editProductCategory, productCategoryId);
                if (res)
                {
                    TempData[SuccessMessage] = "دسته بندی مورد نظر با موفقیت ویرایش شد";
                    return RedirectToAction("Index");
                }
            }
            return View(editProductCategory);
        }

        #endregion

        #region delete category

        [HttpGet("product-category-delete/{productCategoryId}")]
        public async Task<IActionResult> DeleteProductCategory(int productCategoryId)
        {
            var res = await _productService.DeleteProductCategoryInAdmin(productCategoryId);
            if (res)
                TempData[SuccessMessage] = "دسته بندی با موفقیت حذف شد";
            else
                TempData[ErrorMessage] = "در حذف دسته بندی مشکلی به وجود آمد";

            return RedirectToAction("Index");
        }

        #endregion
    }
}
