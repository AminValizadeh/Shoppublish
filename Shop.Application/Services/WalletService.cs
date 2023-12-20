using Shop.Application.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;

namespace Shop.Application.Services
{
    public class WalletService : IWalletService
    {

        #region constructor

        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }


        #endregion

        public async Task<int> DepositMoneyIntoWallet(CreateWalletViewModel createWallet)
        {
            Wallet wallet = new Wallet
            {
                UserId = createWallet.UserId,
                Price = createWallet.Price,
                TransactionType = TransactionType.Deposit,
                Description = createWallet.Description,
                IsPaid = createWallet.IsPaid,
            };

            await _walletRepository.AddWallet(wallet);

            return wallet.Id;
        }

        public async Task<List<Wallet>> GetWalletsByUserId(int userId)
        {
           return await _walletRepository.GetAllWalletByUserId(userId);
        }

        public async Task WithdrawMoneyFromWallet(CreateWalletViewModel createWallet)
        {
            Wallet wallet = new Wallet
            {
                UserId = createWallet.UserId,
                Price = createWallet.Price,
                TransactionType = TransactionType.Withdrawal,
                Description = createWallet.Description,
                IsPaid = createWallet.IsPaid,
            };

            await _walletRepository.AddWallet(wallet);
        }

        public async Task<int> ChargeWallet(ChargeWalletViewModel charge, int userId)
        {
            Wallet wallet = new Wallet
            {
                UserId = userId,
                Price = charge.Amount,
                TransactionType = TransactionType.Deposit,
                Description = "شارژ کیف پول",
                IsPaid = false,
            };

            await _walletRepository.AddWallet(wallet);

            return wallet.Id;
        }

        public async Task<bool> SuccessPaidWallet(int walletId)
        {
            var wallet = await _walletRepository.GetWalletById(walletId);

            if(wallet == null || wallet.IsDelete) return false;
            wallet.IsPaid = true;
            await _walletRepository.UpdateWaallet(wallet);
            return true;
        }

        public async Task<int?> GetAmountWalletByWalletId(int walletId, int userId)
        {
            return await _walletRepository.GetAmountWalletByWalletId(walletId, userId);
        }

        public async Task<int> SumWalletDepositsByUserId(int userId)
        {
            return await _walletRepository.SumWalletDepositsByUserId(userId);
        }

        public async Task<int> SumWalletWithdrawalsByUserId(int userId)
        {
            return await _walletRepository.SumWalletWithdrawalsByUserId(userId);
        }
    }
}
