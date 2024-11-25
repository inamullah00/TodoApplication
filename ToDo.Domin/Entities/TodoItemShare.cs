using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class TodoItemShare
    {

        public Guid Id { get; set; }

        // Foreign Key to TodoItem
        public Guid? TodoItemId { get; set; }
        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get; set; }

        // Foreign Key to ApplicationUser
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
