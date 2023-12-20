using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.UserPanel;

namespace Shop.Application.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterUserResult> AddUserAsync(RegisterUserViewModel register);

        Task<LoginResult> StatusUserForLoginAsync(LoginViewModel login);
        Task<bool> ActiveAccountByEmailActiveCodeAsync(string emailActiveCode);
        Task<UserInformationViewModel> GetUserByIdForUserPanelAsync(int id);
        Task<EditUserPanelViewModel> GetUserByIdForEditUserPanelAsync(int id);
        Task<bool> EditUserInUserPanelAsync(EditUserPanelViewModel editUser, int userId);
        Task<FilterUsersInAdminViewModel> GetUsersForAdminAsync(FilterUsersInAdminViewModel filter);
        Task<CreateUserByAdminResult> AddUserByAdminAsync(CreateUserByAdminViewModel createUser);
        Task<UpdateUserByAdminViewModel?> GetUserForEditByAdminAsync(int id);
        Task<bool> UpdateUserByAdminAsync(UpdateUserByAdminViewModel updateUser);
        Task<User> GetUserByEmailAsync(string email);
        Task<ForgotPasswordResult> GetForgotPasswordByEmailAsync(ForgotPasswordViewModel forgot);
        Task<bool> DeleteUserById(int id);
        Task<bool> ResetPassword(ResetPasswordViewModel reset, string activeCode);
    }
}
