using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Ticket;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.UserPanel.Controllers
{

    public class TicketController : UserBaseController
    {
        #region constructor

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region List

        [HttpGet("tickets")]
        public async Task<IActionResult> Index(FilterTicketViewModel filter)
        {
            filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DES;

            return View(await _ticketService.FilterTickets(filter));
        }

        #endregion

        #region Add ticket

        [HttpGet("add-ticket")]
        public IActionResult AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                var userId = User.GetUserId();
                var result = await _ticketService.AddUserTicket(ticket,userId);

                switch (result)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "مشکلی در ثبت تیک وجود دارد";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیک شما با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }

            return View(ticket);
        }

        #endregion

        #region show ticket detail

        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(int ticketId)
        {
            var userId = User.GetUserId();
            var ticket = await _ticketService.GetTicketForShow(ticketId, userId);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        #endregion

        #region answer ticket

        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewAnswer(AnswerTicketViewModel answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
                ModelState.AddModelError("Text", "لطفا متن خود را وارد  کنید");

            if (ModelState.IsValid)
            {
                var userId = User.GetUserId();
                var res = await _ticketService.AnswerTicket(answer, userId);
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "این تیکت متعلق به شما نیست";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "تیکتی برای ثبت پاسخ پیدا نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = $"پاسخ شما برای تیکت با موفقیت ثبت شد";
                        break;
                }
            }
            return RedirectToAction("TicketDetail", "Ticket", new { area = "UserPanel", ticketId = answer.Id });
        }

        #endregion
    }
}
