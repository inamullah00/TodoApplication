using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Domin.Entities;
using ToDo.Presentation.ViewModels.Comment;

namespace ToDo.Presentation.ViewModels.Item
{
    public class ItemResponseViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public string? UserId { get; set; }
        // Updated to use CommentsViewModel
        public List<CommentsViewModel> Comments { get; set; } = new List<CommentsViewModel>();
    }
}
