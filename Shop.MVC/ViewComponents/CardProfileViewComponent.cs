using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.UserPanel;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.ViewComponents
{
    public class CardProfileViewComponent : ViewComponent
    {
        #region constructor

        private readonly IAccountService _accountService;
        private readonly IWalletService _walletService;

        public CardProfileViewComponent(IAccountService accountService, IWalletService walletService)
        {
            _accountService = accountService;
            _walletService = walletService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserInformationViewModel? userInformation= null;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();
                userInformation =await _accountService.GetUserByIdForUserPanelAsync(userId);

                var sumDeposits = await _walletService.SumWalletDepositsByUserId(User.GetUserId());
                var sumWithdrawals = await _walletService.SumWalletWithdrawalsByUserId(User.GetUserId());
                userInformation.WalletBalance = sumDeposits - sumWithdrawals;
            }
            return View("CardProfile",userInformation);
        }
    }
}
