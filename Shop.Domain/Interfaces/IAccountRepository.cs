using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;

namespace Shop.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> IsUserExistByEmailAsync(string email);
        Task<int> AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetUserByEmailActiveCodeAsync(string emailActiveCode);
        Task SaveChangesAsync();
        Task<User> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<FilterUsersInAdminViewModel> FilterUsers(FilterUsersInAdminViewModel filter);
        Task<User> GetUserByEmailAsync(string email);

        Task<List<User>> FilterUserByEmail(string email);
    }
}
