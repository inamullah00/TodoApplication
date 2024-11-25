using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class TodoItemComment
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; }

        // Foreign Key to ApplicationUser
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Foreign Key to TodoItem
        public Guid ? TodoItemId { get; set; }
        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
