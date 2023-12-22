using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Product;
using static Shop.Domain.ViewModels.Product.FilterProductViewModel;



namespace Shop.MVC.ViewComponents
{


    #region SearchBox
    public class HeaderSearchBoxViewComponent : ViewComponent
    {
        #region Cons

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public HeaderSearchBoxViewComponent(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(FilterProductViewModel filter)
        {



            ViewData["Categories"] = await _productService.GetAllCategories();
            filter = await _productService.FilterProducts(filter);
            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0) return null;

            return View("HeaderSearchBox", filter);
        }
    }
    #endregion

    #region banner one

    public class BannerOneViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BannerOne");
        }
    }

    #endregion

    #region banner two

    public class BannerTwoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BannerTwo");
        }
    }

    #endregion

    #region banner three

    public class BannerThreeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BannerThree");
        }
    }

    #endregion

    #region banner four

    public class BannerFourViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BannerFour");
        }
    }

    #endregion

    #region banner five

    public class BannerFiveViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("BannerFive");
        }
    }

    #endregion
}


