using Shop.Domain.Models.Account;
using Shop.Domain.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.Wallet
{
    public class Wallet : BaseEntity
    {
        #region properties

        public int UserId { get; set; }

        public int Price { get; set; }

        public TransactionType TransactionType { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }

        public bool IsPaid { get; set; }

        #endregion

        #region relations

        public User User { get; set; }

        #endregion
    }

    public enum TransactionType
    {
        [Display(Name = "واریز")]
        Deposit,
        [Display(Name = "برداشت")]
        Withdrawal
    }
}
