using Shop.Domain.Models.Wallet;

namespace Shop.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<List<Wallet>> GetAllWalletByUserId(int userId);
        Task<Wallet> GetWalletById(int id);
        Task AddWallet(Wallet wallet);
        Task UpdateWaallet(Wallet wallet);
        Task SaveChange();
        Task<int?> GetAmountWalletByWalletId(int walletId, int userId);
        Task<int> SumWalletDepositsByUserId(int userId);
        Task<int> SumWalletWithdrawalsByUserId(int userId);
    }
}
