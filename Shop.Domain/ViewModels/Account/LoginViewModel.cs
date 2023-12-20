using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRememberMe { get; set; }
      
    }
    public enum LoginResult
    {
        Success,
        NotExistUser,
        IsNotActive,
        IsBan

    }
}
