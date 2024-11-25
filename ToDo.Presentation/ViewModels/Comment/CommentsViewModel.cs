using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Domin.Entities;

namespace ToDo.Presentation.ViewModels.Comment
{
    public class CommentsViewModel
    {

        public Guid Id { get; set; }
        public string CommentText { get; set; }

        public string? UserId { get; set; }

        public Guid? TodoItemId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
