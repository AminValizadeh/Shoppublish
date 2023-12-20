using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Wallet;

namespace Shop.Data.Repository
{
    public class WalletRepository : IWalletRepository
    {

        #region construtor

        private readonly ShopContext _context;

        public WalletRepository(ShopContext context)
        {
            _context = context;
        }

        #endregion

        public async Task AddWallet(Wallet wallet)
        {
            wallet.CreateDate = DateTime.Now;
            await _context.AddAsync(wallet);
            await SaveChange();
        }

        public async Task<List<Wallet>> GetAllWalletByUserId(int userId)
        {
            return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<int?> GetAmountWalletByWalletId(int walletId, int userId)
        {

            return await _context.Wallets.Where(w => w.Id == walletId && w.UserId == userId && !w.IsPaid).Select(w => w.Price).FirstOrDefaultAsync();
        }

        public async Task<Wallet> GetWalletById(int id)
        {
            return await _context.Wallets.FindAsync(id);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWaallet(Wallet wallet)
        {
            _context.Update(wallet);
            await SaveChange();
        }

        public async Task<int> SumWalletDepositsByUserId(int userId)
        {
            var sumWalletDeposits = await _context.Wallets.Where(w=> w.UserId == userId && w.TransactionType == TransactionType.Deposit && w.IsPaid).Select(w=> w.Price).ToListAsync();
            return sumWalletDeposits.Sum(); 
        }

        public async Task<int> SumWalletWithdrawalsByUserId(int userId)
        {
            var sumWalletWithdrawals = await _context.Wallets.Where(w => w.UserId == userId && w.TransactionType == TransactionType.Withdrawal && w.IsPaid).Select(w => w.Price).ToListAsync();
            return sumWalletWithdrawals.Sum();
        }
    }
}
