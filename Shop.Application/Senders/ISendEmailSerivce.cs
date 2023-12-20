using Microsoft.AspNetCore.DataProtection;
using Shop.Application.Convertors;
using Shop.Application.Utils;
using Shop.Domain.ViewModels.Send;
using TopLearn.Application.Senders;

namespace Shop.Application.Senders
{
    public interface ISendEmailSerivce
    {
        void SendActiveCodeByEmail(string email,string fullName, string activeCode, string viewName, string title);
        void SendActiveCodeByEmailForForgotPassword(string email, string activeCode, string viewName, string title);
    }

    public class SendEmailSerivce : ISendEmailSerivce
    {
        private IViewRenderService _viewRender;

        public SendEmailSerivce(IViewRenderService viewRender)
        {
            _viewRender = viewRender;
        }

        public void SendActiveCodeByEmail(string email, string fullName, string activeCode, string viewName, string title)
        {
            email = email.FixedEmail();

            SendEmailViewModel emailModel = new SendEmailViewModel
            {
                FullName = fullName,
                ActiveCode = activeCode
            };
            string body = _viewRender.RenderToStringAsync(viewName, emailModel);
            SendEmail.Send(email, title, body);
        }

        public void SendActiveCodeByEmailForForgotPassword(string email, string activeCode, string viewName, string title)
        {
            email = email.FixedEmail();

            SendEmailForForgotPasswordViewModel emailModel = new SendEmailForForgotPasswordViewModel
            {
                ActiveCode = activeCode
            };
            string body = _viewRender.RenderToStringAsync(viewName, emailModel);
            SendEmail.Send(email, title, body);
        }
    }
}
