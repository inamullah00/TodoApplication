
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domin.Entities
{
    public class ApplicationUser : IdentityUser
    {



        // Navigation properties (one-to-many relationships)
        public ICollection<TodoItem>? TodoItems { get; set; }
        public ICollection<TodoItemComment>? Comments { get; set; }
        public ICollection<TodoItemShare>? SharedTodoItems { get; set; }
    }
}
