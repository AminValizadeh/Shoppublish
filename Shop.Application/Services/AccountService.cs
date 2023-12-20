using Shop.Application.Extensions;
using Shop.Application.Interfaces;
using Shop.Application.Senders;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.UserPanel;

namespace Shop.Application.Services
{
    public class AccountService : IAccountService
    {
        #region constructor

        private readonly IAccountRepository _accountRepository;
        private readonly ISendEmailSerivce _sendEmailSerivce;
        private readonly IRolePermissionService _rolePermissionService;


        public AccountService(IAccountRepository accountRepository, ISendEmailSerivce sendEmailSerivce, IRolePermissionService rolePermissionService)
        {
            _accountRepository = accountRepository;
            _sendEmailSerivce = sendEmailSerivce;
            _rolePermissionService = rolePermissionService;
        }

        #endregion

        public async Task<RegisterUserResult> AddUserAsync(RegisterUserViewModel register)
        {
            if (await _accountRepository.IsUserExistByEmailAsync(register.Email))
                return RegisterUserResult.ExistUser;

            User user = new User
            {
                Email = register.Email.FixedEmail(),
                FullName = register.FullName,
                Password = register.Password.EncodePasswordMd5(),
                EmailActiveCode = Guid.NewGuid().ToString(),
                IsEmailActive = true
            };
            await _accountRepository.AddAsync(user);

           

            return RegisterUserResult.Success;
        }

        public async Task<LoginResult> StatusUserForLoginAsync(LoginViewModel login)
        {
            var user = await _accountRepository.GetUserByEmailAndPasswordAsync(login.Email, login.Password.EncodePasswordMd5());

            if (user == null || user.IsDelete) return LoginResult.NotExistUser;

            if (!user.IsEmailActive) return LoginResult.IsNotActive;

            if (user.IsBan) return LoginResult.IsBan;

            return LoginResult.Success;
        }

        public async Task<bool> ActiveAccountByEmailActiveCodeAsync(string emailActiveCode)
        {
            var user = await _accountRepository.GetUserByEmailActiveCodeAsync(emailActiveCode);
            if (user == null || user.IsEmailActive || user.EmailActiveCode != emailActiveCode) return false;
            user.IsEmailActive = true;
            user.EmailActiveCode = Guid.NewGuid().ToString();
            await _accountRepository.UpdateAsync(user);
            return true;
        }

        public async Task<UserInformationViewModel> GetUserByIdForUserPanelAsync(int id)
        {
            var user = await _accountRepository.GetUserByIdAsync(id);
            return new UserInformationViewModel
            {
                FullName = user?.FullName ?? "",
                Mobile = user?.Mobile,
                ImageName = user?.ImageName,
            };
        }

        public async Task<EditUserPanelViewModel> GetUserByIdForEditUserPanelAsync(int id)
        {
            var user = await _accountRepository.GetUserByIdAsync(id);
            return new EditUserPanelViewModel
            {
                FullName = user?.FullName ?? "",
                Mobile = user?.Mobile,
                ImageName = user?.ImageName,
            };
        }

        public async Task<bool> EditUserInUserPanelAsync(EditUserPanelViewModel editUser, int userId)
        {
            var user = await _accountRepository.GetUserByIdAsync(userId);

            if (user != null)
            {
                user.FullName = editUser.FullName;
                user.Mobile = editUser.Mobile;

                if (editUser.Avatar != null && editUser.Avatar.IsImage())
                {
                    var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(editUser.Avatar.FileName);
                    editUser.Avatar.AddImageToServer(
                        imageName,
                        PathExtension.UserAvatarOriginServer,
                        100,
                        100,
                        PathExtension.UserAvatarThumbServer,
                        editUser.ImageName
                        );
                    user.ImageName = imageName;
                }

                await _accountRepository.UpdateAsync(user);

                return true;
            }

            return false;
        }

        public async Task<FilterUsersInAdminViewModel> GetUsersForAdminAsync(FilterUsersInAdminViewModel filter)
        {
            return await _accountRepository.FilterUsers(filter);
        }

        public async Task<CreateUserByAdminResult> AddUserByAdminAsync(CreateUserByAdminViewModel createUser)
        {
            if (await _accountRepository.IsUserExistByEmailAsync(createUser.Email))
                return CreateUserByAdminResult.ExistUser;

            User user = new User
            {
                Email = createUser.Email.FixedEmail(),
                FullName = createUser.FullName,
                Password = createUser.Password.EncodePasswordMd5(),
                EmailActiveCode = Guid.NewGuid().ToString(),
                IsEmailActive = true,
                Mobile = createUser.Mobile,
                IsAdmin = createUser.IsAdmin
            };

            #region insert image

            if (createUser.Avatar != null && createUser.Avatar.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(createUser.Avatar.FileName);
                createUser.Avatar.AddImageToServer(
                    imageName,
                    PathExtension.UserAvatarOriginServer,
                    100,
                    100,
                    PathExtension.UserAvatarThumbServer,
                    null
                    );
                user.ImageName = imageName;
            }

            #endregion

            await _accountRepository.AddAsync(user);

            // add role to user
            await _rolePermissionService.AddRolesToUser(createUser.SelectedRoles, user.Id);

            return CreateUserByAdminResult.Success;
        }

        public async Task<UpdateUserByAdminViewModel?> GetUserForEditByAdminAsync(int id)
        {
            User? user = await _accountRepository.GetUserByIdAsync(id);
            if (user == null) return null;
            List<int> SelectedUserRoles = await _rolePermissionService.SelectedRoleIDsByUserId(user.Id);
            return new UpdateUserByAdminViewModel
            {
                ImageName = user.ImageName,
                FullName = user.FullName,
                Id = user.Id,
                IsAdmin = user.IsAdmin,
                IsBan = user.IsBan,
                IsEmailActive = user.IsEmailActive,
                Mobile = user.Mobile,
                SelectedRoles = SelectedUserRoles
            };
        }

        public async Task<bool> UpdateUserByAdminAsync(UpdateUserByAdminViewModel updateUser)
        {
            User? user = await _accountRepository.GetUserByIdAsync(updateUser.Id);
            if (user == null) return false;

            user.IsAdmin = updateUser.IsAdmin;
            user.IsBan = updateUser.IsBan;
            user.FullName = updateUser.FullName;
            user.Mobile = updateUser.Mobile;
            user.IsEmailActive = updateUser.IsEmailActive;

            if (!string.IsNullOrEmpty(updateUser.Password))
                user.Password = updateUser.Password.EncodePasswordMd5();

            #region insert image


            if (updateUser.Avatar != null && updateUser.Avatar.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(updateUser.Avatar.FileName);
                updateUser.Avatar.AddImageToServer(
                    imageName,
                    PathExtension.UserAvatarOriginServer,
                    100,
                    100,
                    PathExtension.UserAvatarThumbServer,
                    updateUser.ImageName
                    );
                user.ImageName = imageName;
            }

            #endregion

            await _accountRepository.UpdateAsync(user);

            // update user role
            await _rolePermissionService.EditRolesUser(updateUser.SelectedRoles,user.Id);

            return true;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _accountRepository.GetUserByEmailAsync(email.FixedEmail());
        }


        public async Task<bool> DeleteUserById(int id)
        {
            var user = await _accountRepository.GetUserByIdAsync(id);
            if (user == null || user.IsDelete)
            {
                return false;
            }
            user.IsDelete = true;
            await _accountRepository.UpdateAsync(user);
            return true;
        }


        public async Task<ForgotPasswordResult> GetForgotPasswordByEmailAsync(ForgotPasswordViewModel forgot)
        {
            var user = await _accountRepository.GetUserByEmailAsync(forgot.Email);
            if (user == null) return ForgotPasswordResult.NotFound;
            if (user.IsDelete) return ForgotPasswordResult.Deleted;
            if (user.IsBan) return ForgotPasswordResult.Banded;
            if (user.IsEmailActive == false) return ForgotPasswordResult.NotEmailActive;


            //SenEmail
            _sendEmailSerivce.SendActiveCodeByEmailForForgotPassword(user.Email, user.EmailActiveCode, "_ActiveEmailForgotPasswordView", "تغییر کلمه عبور حساب کاربری");
            return ForgotPasswordResult.Success;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel reset, string activeCode)
        {
            var user = await _accountRepository.GetUserByEmailActiveCodeAsync(activeCode);
            if (user == null) return false;
            user.Password = reset.Password.EncodePasswordMd5();
            user.EmailActiveCode = Guid.NewGuid().ToString();
            await _accountRepository.UpdateAsync(user);
            return true;
        }
    }
}
