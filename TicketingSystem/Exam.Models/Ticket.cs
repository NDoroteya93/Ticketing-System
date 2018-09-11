using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ApplicationUser Assignee { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        public Priority Priority { get; set; }

        public TaskState TaskState { get; set; }

        public string ScreenshotUrl { get; set; }

        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Ticket Tickets { get; set; }

        public Ticket()
        {
            this.Comments = new HashSet<Comment>();
        }
    }
}
