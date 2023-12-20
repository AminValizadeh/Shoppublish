using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Ticket
{
    public class AnswerTicketViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; } = string.Empty;
    }

    public enum AnswerTicketResult
    {
        NotForUser,
        NotFound,
        Success
    }

}
