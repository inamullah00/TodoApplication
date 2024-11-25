using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }

        // Foreign Key to ApplicationUser
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        // Navigation properties (one-to-many relationships)
        public ICollection<TodoItemComment> Comments { get; set; }
        public ICollection<TodoItemShare> SharedWith { get; set; }
        public ICollection<TodoItemReminder> Reminders { get; set; }
        public ICollection<RecurringTodoItem> RecurringTasks { get; set; }
    }
}
