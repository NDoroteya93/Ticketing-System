using TicketingSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TicketingSystem.Models;
using Microsoft.AspNet.Identity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TicketingSystem.Web.Controllers
{
    public class TicketsController : BaseController
    {
        public ActionResult Details(int id)
        {
            //var ticket = this.Data.Tickets.All().Include(t => t.Category).Include(t => t.Comments).Include(t => t.Tickets).Include(t => t.Assignee).FirstOrDefault(t => t.Id == id);
            var ticket = this.Data.Tickets.GetById(id);
            var allChild = this.Data.Tickets.All().Where(t => t.Tickets.Id == id).Select(TicketBasicViewModel.FromTicket).ToList();
            var ticketViewModel = new TicketDetailsViewModel()
            {
                Id = ticket.Id,
                Author = ticket.Author.UserName,
                Category = ticket.Category.Name,
                Description = ticket.Description,
                ScreenshotUrl = ticket.ScreenshotUrl,
                Title = ticket.Title,
                Priority = ticket.Priority.ToString(),
                Comments = ticket.Comments.AsQueryable().Select(CommentBasicViewModel.FromComment).ToList(),
                Tickets = allChild,
                Assignee = ticket.Assignee == null ? string.Empty : ticket.Assignee.UserName,
                TaskState = ticket.TaskState,
                CreateDate = ticket.CreateDate,
                EndDate = ticket.EndDate
            };

            return View(ticketViewModel);
        }

        [Authorize]
        public ActionResult Create(int? Id)
        {
            ViewBag.PriorityId = new SelectList(EnumsController.EnumToDropDownList(typeof(Priority)), "Value", "Text");
            ViewBag.CategoryId = new SelectList(this.Data.Categories.All(), "Id", "Name");
            ViewBag.TicketId = Id;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create(TicketCreateViewModel ticket)
        {
            ViewBag.PriorityId = new SelectList(EnumsController.EnumToDropDownList(typeof(Priority)), "Value", "Text");
            ViewBag.CategoryId = new SelectList(this.Data.Categories.All(), "Id", "Name");

            if (User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                using (TransactionScope tsTransScope = new TransactionScope())
                {
                    Ticket parentTicket = null;
                    var currentUserId = User.Identity.GetUserId();

                    var currentUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == currentUserId);
                    currentUser.Points = currentUser.Points + 1;
                    if (ticket.Ticket_Id != 0)
                    {
                        parentTicket = this.Data.Tickets.GetById(ticket.Ticket_Id);
                        // parentTicket.Add(parentTicketObjet);
                    }

                    var newTicket = new Ticket()
                    {
                        AuthorId = currentUserId,
                        CategoryId = ticket.CategoryId,
                        Description = HttpUtility.HtmlEncode(ticket.Description),
                        Priority = (Priority)ticket.PriorityId,
                        ScreenshotUrl = ticket.ScreenshotUrl,
                        Title = ticket.Title,
                        Tickets = parentTicket,
                        CreateDate = DateTime.UtcNow
                    };

                    this.Data.Tickets.Add(newTicket);
                    this.Data.SaveChanges();
                    tsTransScope.Complete();
                }

                return RedirectToAction("TicketsList");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public string AssigneeUserToTask(int Id, string UserId)
        {
            using (TransactionScope tsTransScope = new TransactionScope())
            {
                var assignedUser = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == UserId);

                var updateTicket = this.Data.Tickets.GetById(Id);
                ApplicationUser nullUser = new ApplicationUser()
                {
                    Id = "0",
                    UserName = ""
                };

                updateTicket.Assignee = assignedUser == null ? nullUser : assignedUser;

                this.Data.Tickets.Update(updateTicket);
                this.Data.SaveChanges();
                tsTransScope.Complete();
                return assignedUser == null ? string.Empty : assignedUser.UserName;
            }
        }

        public ActionResult AllTickets([DataSourceRequest] DataSourceRequest request)
        {
            var allTickets = this.Data.Tickets.All().Select(TicketListViewModel.FromTicket);
            var listOfAllTickets = new List<TicketListReturnViewModel>();

            foreach (var ticket in allTickets)
            {
                var curentTicket = new TicketListReturnViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Author = ticket.Author,
                    Category = ticket.Category,
                    Priority = Enum.GetName(typeof(Priority), ticket.Priority)
                };

                listOfAllTickets.Add(curentTicket);
            }

            return Json(listOfAllTickets.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TicketsList()
        {
            var allTickets = this.Data.Tickets.All().Select(TicketListViewModel.FromTicket);
            var listOfAllTickets = new List<TicketListReturnViewModel>();

            foreach (var ticket in allTickets)
            {
                var curentTicket = new TicketListReturnViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Author = ticket.Author,
                    Category = ticket.Category,
                    Priority = Enum.GetName(typeof(Priority), ticket.Priority)
                };

                listOfAllTickets.Add(curentTicket);
            }

            return View(listOfAllTickets);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SubTicket(CommentCreateViewModel comment)
        {
            if (User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var newComment = new Comment()
                {
                    AuthorId = currentUserId,
                    Content = comment.Content,
                    TicketId = comment.Id
                };

                this.Data.Comments.Add(newComment);
                this.Data.SaveChanges();

                var currentUser = User.Identity.Name;

                return Content("<p><strong>" + currentUser + ": </strong>" + comment.Content + "</p>");
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Comment(CommentCreateViewModel comment)
        {
            if (User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var newComment = new Comment()
                {
                    AuthorId = currentUserId,
                    Content = comment.Content,
                    TicketId = comment.Id
                };

                this.Data.Comments.Add(newComment);
                this.Data.SaveChanges();

                var currentUser = User.Identity.Name;

                return Content("<p><strong>" + currentUser + ": </strong>" + comment.Content + "</p>");
            }

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, ModelState.Values.First().ToString());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(int? searchedCategory)
        {
            if (searchedCategory == null)
            {
                var resultsAll = this.Data.Tickets.All()
                .Select(TicketBasicViewModel.FromTicket);
                return View(resultsAll);
            }
            else
            {
                var results = this.Data.Tickets.All()
                .Where(t => t.CategoryId == searchedCategory)
                .Select(TicketBasicViewModel.FromTicket);
                return View(results);
            }
        }

        [Authorize]
        public ActionResult CategoriesList()
        {
            var categories = this.Data.Categories.All().Select(x => new { Id = x.Id, Name = x.Name });
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetUsers()
        {
            List<UsersViewModel> users = new List<UsersViewModel>();
            UsersViewModel unassignedUser = new UsersViewModel()
            {
                Id = "0",
                UserName = "Unassign"
            };

            users = this.Data.ApplicationUsers.All().Select(x => new UsersViewModel() { Id = x.Id, UserName = x.UserName }).ToList();
            users.Add(unassignedUser);
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        public ContentResult ChangeState(int Id, string TaskState)
        {
            var ticket = this.Data.Tickets.GetById(Id);
            TaskState ticketState = TicketingSystem.Models.TaskState.ToDo;
            if (TaskState == "ToDo")
            {
                ticketState = (TaskState)System.Enum.Parse(typeof(TaskState), "InProgress");
            }
            else if (TaskState == "InProgress")
            {
                ticketState = (TaskState)System.Enum.Parse(typeof(TaskState), "ToDo");
            }
            else if (TaskState == "Done")
            {
                ticketState = (TaskState)System.Enum.Parse(typeof(TaskState), "Done");
                if (ticket.Assignee != null)
                {
                    var UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                    var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.Id == UserId);
                    ticket.Assignee = user;
                    ticket.EndDate = DateTime.UtcNow;
                }
            }

            ticket.TaskState = ticketState;

            this.Data.Tickets.Update(ticket);
            this.Data.SaveChanges();

            var jsonTicketState = JsonConvert.SerializeObject(ticketState, Formatting.None, new JsonSerializerSettings { Converters = new[] { new StringEnumConverter() } });

            return this.Content(jsonTicketState, "application/json");
        }
    }

    public class EnumsController : BaseController
    {
        [NonAction]
        public static List<SelectListItem> EnumToDropDownList(Type enumType)
        {
            return Enum
              .GetValues(enumType)
              .Cast<int>()
              .Select(i => new SelectListItem
              {
                  Value = i.ToString(),
                  Text = Enum.GetName(enumType, i),
              }
              )
              .ToList();
        }
    }
}