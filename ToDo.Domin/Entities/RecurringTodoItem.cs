using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class RecurringTodoItem
    {
        public Guid Id { get; set; }

        // Foreign Key to TodoItem
        public Guid ? TodoItemId { get; set; }

        [ForeignKey(nameof(TodoItemId))]
        public TodoItem TodoItem { get; set; }

        public string RecurrenceType { get; set; } // e.g., "Daily", "Weekly", etc.
        public DateTime NextDueDate { get; set; }
       
    }
}
