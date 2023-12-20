using Microsoft.AspNetCore.Mvc;

namespace Shop.MVC.ViewComponents
{
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
