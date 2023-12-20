namespace Shop.Domain.ViewModels.UserPanel
{
    public class UserInformationViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string? Mobile { get; set; }
        public string? ImageName { get; set; }
        public int WalletBalance { get; set; }
        public int Score { get; set; }
    }
}
