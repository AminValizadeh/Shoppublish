using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Models.Common;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.UserPanel.Controllers
{
    public class WalletController : UserBaseController
    {

        #region constructor

        private readonly IWalletService _walletService;
        private readonly IPaymentService _paymentService;

        public WalletController(IWalletService walletService, IPaymentService paymentService)
        {
            _walletService = walletService;
            _paymentService = paymentService;
        }

        #endregion

        #region index

        [HttpGet("wallets")]
        public async Task<IActionResult> Index()
        {
            int userId = User.GetUserId();
            List<Wallet> wallets = await _walletService.GetWalletsByUserId(userId);
            return View(wallets);
        }

        #endregion

        #region charge wallet

        [HttpPost("wallets/charge-wallet"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel charge)
        {
            if (ModelState.IsValid)
            {
                int walletId = await _walletService.ChargeWallet(charge, User.GetUserId());

                string callbackUrl = PathExtension.DomainAddressHtttps + Url.RouteUrl("WalletZarinPalPaymentResult", new { walletId });

                string redirectUrl = "";

                var status = _paymentService.CreatePaymentRequest(
                    null,
                    charge.Amount,
                    "تکمیل فرایند خرید از سایت",
                    callbackUrl,
                   ref redirectUrl);

                if (status == PaymentStatus.St100)
                {
                    return Redirect(redirectUrl);
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region call back zarinpal

        [HttpGet("wallets/payment-result/{walletId}", Name = "WalletZarinPalPaymentResult")]
        public async Task<IActionResult> CallBackZarinPal(int walletId)
        {
            string? authority = _paymentService.GetAuthorityCodeFromCallback(HttpContext);
            if (authority == null)
            {
                TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
                return RedirectToAction("Index");
            }

            var amountWallet = await _walletService.GetAmountWalletByWalletId(walletId, User.GetUserId());

            if (amountWallet != null)
            {
                long refId = 0;
                var res = _paymentService.PaymentVerification(null, authority, amountWallet.Value, ref refId);
                if (res == PaymentStatus.St100)
                {
                    TempData[SuccessMessage] = "پرداخت شما با موفقیت انجام شد";
                    TempData[InfoMessage] = "کد پیگیری شما : " + refId;
                    var resultPaidWallet = await _walletService.SuccessPaidWallet(walletId);
                    if (resultPaidWallet)
                        TempData[SuccessMessage] = "وضعیت پرداخت با موفقیت تایید شد";
                    else
                        TempData[ErrorMessage] = "در تایید پرداخت مشکلی پیش امد";

                    return RedirectToAction("Index");
                }
            }

            TempData[WarningMessage] = "عملیات پرداخت با شکست مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion
    }
}
