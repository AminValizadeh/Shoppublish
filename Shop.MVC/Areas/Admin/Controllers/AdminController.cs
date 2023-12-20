using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Application.Security;

namespace Shop.MVC.Areas.Admin.Controllers
{
 
    public class AdminController : AdminBaseController
    {
        #region constructor

        private readonly IOrderService _orderService;

        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion

    
        public async Task<IActionResult> Index()
        {
            ViewData["ProductBestSellerLastTenDays"] = await _orderService.GetProductBestSellerLastTenDays();
            ViewData["ProductBestSellerCurrentMonth"] = await _orderService.GetProductBestSellerCurrentMonthShamsi();
            return View();
        }
    }
}
