using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;

namespace Shop.Application.Interfaces
{
    public interface IWalletService
    {
        Task<List<Wallet>> GetWalletsByUserId(int userId);
        Task<int> DepositMoneyIntoWallet(CreateWalletViewModel createWallet);
        Task WithdrawMoneyFromWallet(CreateWalletViewModel createWallet);
        Task<int> ChargeWallet(ChargeWalletViewModel charge, int userId);
        Task<bool> SuccessPaidWallet(int walletId);
        Task<int?> GetAmountWalletByWalletId(int walletId, int userId);
        Task<int> SumWalletDepositsByUserId(int userId);
        Task<int> SumWalletWithdrawalsByUserId(int userId);
    }
}
