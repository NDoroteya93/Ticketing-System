﻿using TicketingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TicketingSystem.Web.Models
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Priority { get; set; }

        public string ScreenshotUrl { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TaskState TaskState { get; set; }

        public ICollection<CommentBasicViewModel> Comments { get; set; }

        public ICollection<TicketBasicViewModel> Tickets { get; set; }

        //public static Expression<Func<Ticket, TicketDetailsViewModel>> FromTicket
        //{
        //    get
        //    {
        //        return ticket => new TicketDetailsViewModel()
        //        {
        //            Id = ticket.Id,
        //            Author = ticket.Author.UserName,
        //            Category = ticket.Category.Name,
        //            Description = ticket.Description,
        //            ScreenshotUrl = ticket.ScreenshotUrl,
        //            Title = ticket.Title,
        //            Priority = ticket.Priority,
        //            Comments = ticket.Comments.AsQueryable().Select(CommentBasicViewModel.FromComment).ToList()
        //        };
        //    }
        //}
    }
}