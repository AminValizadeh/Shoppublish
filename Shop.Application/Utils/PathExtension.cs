namespace Shop.Application.Utils
{
    public class PathExtension
    {
        #region domain address

        public static string DomainAddressHtttps = "https://localhost:7150";
        public static string DomainAddressHtttp = "https://localhost:5033";

        #endregion

        #region default images

        public static string DefaultAvatar = "/Template/Images/Defaults/Defult.jpg";

        #endregion

        #region user

        public static string UserAvatarOrigin = "/Template/Images/User/Origin/";
        public static string UserAvatarOriginServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Template/Images/User/Origin/");

        public static string UserAvatarThumb = "/Template/Images/User/Thumb/";
        public static string UserAvatarThumbServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Template/Images/User/Thumb/");

        #endregion

        #region upload ckeditor image tickets

        public static string UploadImageTicket = "/Template/Images/Tickets/Origin/";
        public static string UploadImageTicketServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Template/Images/Tickets/Origin/");

        #endregion

        #region product

        public static string ProductImageOrigin = "/Template/Images/Product/Origin/";
        public static string ProductImageOriginServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Template/Images/Product/Origin/");

        public static string ProductImageThumb = "/Template/Images/Product/Thumb/";
        public static string ProductImageThumbServer =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Template/Images/Product/Thumb/");

        #endregion
    }
}
