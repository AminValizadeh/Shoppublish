using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Models.Common;
using Shop.Domain.ViewModels.Orders;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.UserPanel.Controllers
{
    public class OrderController : UserBaseController
    {
        #region constructor

        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        #endregion

        #region list order

        [HttpGet("order")]
        public async Task<IActionResult> Index()
        {
            var userOpenOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            return View(userOpenOrder);
        }

        #endregion

        #region remove product from order

        [HttpGet("order/remove-product-order/{detailId}/{productId}")]
        public async Task<IActionResult> RemoveProductFromOrder(int detailId, int productId)
        {
            var res = await _orderService.RemoveOrderDetail(detailId, productId);
            if (res)
                TempData[SuccessMessage] = "محصول با موفقیت از سبد خرید حذف شد";
            else
                TempData[ErrorMessage] = "در حذف محصول از سبد خرید مشکلی وجود دارد";
            return RedirectToAction("Index");
        }

        #endregion

        #region change count and discount code product

        [HttpPost("order/change-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeCountAndDiscountCodeProduct(AddProductToOrderViewModel order)
        {
            await _orderService.AddProductToOpenOrder(User.GetUserId(), order);
            TempData[SuccessMessage] = "تغییرات با موفقیت ثبت شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region charge wallet

        [HttpGet("order/pay-order")]
        public async Task<IActionResult> PayOrder()
        {
            int amount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());

            if (amount > 0)
            {
                string callbackUrl = PathExtension.DomainAddressHtttps + Url.RouteUrl("ZarinPalPaymentResult");

                string redirectUrl = "";

                var status = _paymentService.CreatePaymentRequest(
                    null,
                    amount,
                    "تکمیل فرایند خرید از سایت",
                    callbackUrl,
                   ref redirectUrl);

                if (status == PaymentStatus.St100)
                {
                    return Redirect(redirectUrl);
                }
            }
            TempData[ErrorMessage] = "عملیات خرید با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region call back zarinpal

        [HttpGet("payment-result/", Name = "ZarinPalPaymentResult")]
        public async Task<IActionResult> CallBackZarinPal()
        {
            string? authority = _paymentService.GetAuthorityCodeFromCallback(HttpContext);
            if (authority == null)
            {
                TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
                return RedirectToAction("Index");
            }

            int amount = await _orderService.GetTotalOrderPriceForPayment(User.GetUserId());

            if (amount > 0)
            {
                long refId = 0;
                var res = _paymentService.PaymentVerification(null, authority, amount, ref refId);
                if (res == PaymentStatus.St100)
                {
                    TempData[SuccessMessage] = "پرداخت شما با موفقیت انجام شد";
                    TempData[InfoMessage] = "کد پیگیری شما : " + refId;
                    await _orderService.PayOrderProduct(User.GetUserId(), (int)refId);

                    return RedirectToAction("Index");
                }
            }

            TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region purchased products

        [HttpGet("order/purchased-products")]
        public async Task<IActionResult> PurchasedProducts()
        {
            var orders = await _orderService.GetUserShoppingCartsByUserId(User.GetUserId());
            return View(orders);
        }

        #endregion

        #region detail purchased products

        [HttpGet("order/detail-purchased-products/{orderId}")]
        public async Task<IActionResult> DetailPurchasedProducts(int orderId)
        {
            var orderDetails = await _orderService.GetDetailUserShoppingCartsByUserIdAndOrderId(User.GetUserId(),orderId);
            return View(orderDetails);
        }

        #endregion
    }
}
