using TicketingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TicketingSystem.Web.Models
{
    public class TicketBasicViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public int CommentsCount { get; set; }

        public string Description { get; set; }

        public TaskState TaskState { get; set; }

        public Priority Priority { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public static Expression<Func<Ticket, TicketBasicViewModel>> FromTicket
        {
            get
            {
                return ticket => new TicketBasicViewModel()
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Author = ticket.Author.UserName,
                    Category = ticket.Category.Name,
                    CommentsCount = ticket.Comments.Count(),
                    TaskState = ticket.TaskState
                };
            }
        }
    }
}