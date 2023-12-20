using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Domain.Entities.Ticket;
using Shop.Domain.ViewModels.Ticket;
using Shop.MVC.PresentationExtensions;

namespace Shop.MVC.Areas.Admin.Controllers
{
    [Route("admin/ticket/")]
    public class TicketController : AdminBaseController
    {
        #region constructor

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region list ticket

        [HttpGet("ticket-list")]
        public async Task<IActionResult> Index(FilterTicketViewModel filter)
        {
            filter.UserId = null;
            filter = await _ticketService.FilterTickets(filter);
            return View(filter);
        }

        #endregion

        #region detail ticket

        [HttpGet("ticket-detail/{ticketId}")]
        public async Task<IActionResult> TicketDetail(int ticketId)
        {
            var ticket = await _ticketService.GetTicketForShowAdmin(ticketId);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        #endregion

        #region create ticket

        [HttpGet("ticket-create")]
        public IActionResult TicketCreate()
        {
            return View();
        }

        [HttpPost("ticket-create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketCreate(AddTicketByAdminViewModel ticket)
        {
            if (ModelState.IsValid)
            {
                var userId = User.GetUserId();
                var result = await _ticketService.AddTicketFromAdminForUser(ticket, userId);

                switch (result)
                {
                    case AddTicketyAdminResult.Error:
                        TempData[ErrorMessage] = "مشکلی در ثبت تیک وجود دارد";
                        break;
                    case AddTicketyAdminResult.Success:
                        TempData[SuccessMessage] = "تیک شما با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }

            return View(ticket);
        }

        #endregion

        #region get user json

        [HttpGet("user-autocomplete")]
        public async Task<IActionResult> GetSellerProductsJson(string email)
        {
            var data = await _ticketService.FilterUserByEmailForCreateTicketFromAdmin(email);
            return new JsonResult(data);
        }

        #endregion

        #region answer ticket

        [HttpPost("ticket-answer"), ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketAnswer(AnswerTicketViewModel answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
                TempData[ErrorMessage] = "لطفا کادر ادیتور متن قبل از ارسال جواب تیکت پر نمایید";

            if (ModelState.IsValid)
            {
                var userId = User.GetUserId();
                var res = await _ticketService.AnswerTicketFromAdmin(answer, userId);
                switch (res)
                {
                    case AnswerTicketResult.NotFound:
                        TempData[ErrorMessage] = "تیکتی برای ثبت پاسخ پیدا نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = $"پاسخ شما برای تیکت با موفقیت ثبت شد";
                        break;
                }
            }
            return RedirectToAction("TicketDetail", "Ticket", new { area = "Admin", ticketId = answer.Id });
        }

        #endregion

        #region ticket close

        [HttpGet("ticket-closed/{ticketId}")]
        public async Task<IActionResult> TicketClosed(int ticketId)
        {
           bool reslut = await _ticketService.ClosedTicketByTicketId(ticketId);
            if (reslut)
                TempData[SuccessMessage] = "تیکت با موفقیت بسته شد";
            else
                TempData[ErrorMessage] = "تیکیتی پیدا نشد";
            return RedirectToAction("Index");
        }
        
        #endregion
    }
}
