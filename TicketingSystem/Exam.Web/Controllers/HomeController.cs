using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TicketingSystem.Models;
using TicketingSystem.Web.Models;

namespace TicketingSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            var model = await this.LoadTickets();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllTickets()
        {
            var model = await this.LoadTickets();
            return PartialView("_HomePartialView", model);
        }
        public async Task<List<TicketBasicViewModel>> LoadTickets()
        {
            var mostPopularTickets = this.Data
           .Tickets
           .All()
           .OrderByDescending(t => t.Comments.Count())
           .Select(TicketBasicViewModel.FromTicket);
            return mostPopularTickets.ToList();
        }
        public ActionResult DragAndDropItems(int Id, string TicketState)
        {
            var ticket = this.Data.Tickets.GetById(Id);
            string newState = TicketState.Replace("div", "");

            TaskState ticketState = (TaskState)System.Enum.Parse(typeof(TaskState), newState);
            ticket.TaskState = ticketState;

            this.Data.Tickets.Update(ticket);
            this.Data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}