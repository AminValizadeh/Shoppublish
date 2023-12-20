using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Orders;
using Shop.Domain.ViewModels.Product;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Controllers
{
    public class ProductController : SiteBaseController
    {

        #region constructor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ProductController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        #endregion

        #region list product

        [HttpGet("product-list")]
        [HttpGet("products-list/{CategoryId}")]
        public async Task<IActionResult> Index(FilterProductViewModel filter)
        {
            ViewData["Categories"] = await _productService.GetAllCategories();

            filter = await _productService.FilterProducts(filter);

            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0) return NotFound();
            return View(filter);
        }

        #endregion

        #region detail product

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            var productDetail = await _productService.GetProductForShowDetailProductById(productId);
            if (productDetail == null) return NotFound();
            return View(productDetail);
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

        #region add product to order

        [HttpGet("product/add-product-to-order/{productId}")]
        public async Task<IActionResult> AddProductToOrder(int productId)
        {
            AddProductToOrderViewModel addProduct = new AddProductToOrderViewModel();
            addProduct.ProductId = productId;
            addProduct.Count = 1;

            await _orderService.AddProductToOpenOrder(User.GetUserId(),addProduct);

            TempData[SuccessMessage] = "محصول با موفقیت به سبد خرید اضافه شد";

            return RedirectToAction("ProductDetail", new { productId });
        }

        #endregion
    }
}
