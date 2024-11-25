using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class TodoItemReminder
    {
        public Guid Id { get; set; }
        public DateTime ReminderTime { get; set; }
        public bool IsNotified { get; set; } = false;

        // Foreign Key to TodoItem
        public Guid? TodoItemId { get; set; }
        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
