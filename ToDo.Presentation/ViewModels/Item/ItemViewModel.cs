namespace ToDo.Presentation.ViewModels.Item;

public class ItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsComplete { get; set; }
    public string UserId { get; set; }
}
