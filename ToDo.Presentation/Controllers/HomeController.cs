using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;
using ToDo.Application.Services.TodoServices;
using ToDo.Domin.Entities;
using ToDo.Presentation.ViewModels.Comment;
using ToDo.Presentation.ViewModels.Item;

namespace ToDo.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITodoGenericService<TodoItem> _todoGenericService;
        private readonly ITodoGenericService<TodoItemComment> _commentService;

        public HomeController(ITodoGenericService<TodoItem> todoGenericService, ITodoGenericService<TodoItemComment> commentService)
        {
            _todoGenericService = todoGenericService;
            _commentService = commentService;
        }

        // GET: List All Todo Items
    
        [HttpGet]
        [Route("")]
        [Route("TodoItems")]
        public async Task<IActionResult> TodoItems()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(); 
                }

               
                var todoItems = await _todoGenericService.GetAllAsync();
                var userTodoItems = todoItems.Where(item => item.UserId == userId).ToList();

                var itemViewModels = userTodoItems.Select(item => new ItemResponseViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Priority = item.Priority,
                    DueDate = item.DueDate,
                    IsComplete = item.IsComplete,
                    UserId = item.UserId,
                    Comments = item.Comments?.Select(comment => new CommentsViewModel
                    {
                        Id = comment.Id,
                        CommentText = comment.CommentText,
                        UserId = comment.UserId,
                        TodoItemId = comment.TodoItemId,
                        CreatedAt = comment.CreatedAt
                    }).ToList() ?? new List<CommentsViewModel>()
                }).ToList();

                return View(itemViewModels);
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the Todo items.");
                return View(new List<ItemResponseViewModel>());
            }
        }

        // GET: Get Comments for a Specific Todo Item
        [HttpGet("Comment/GetComments/{todoItemId:guid}")]
        public async Task<IActionResult> GetComments(Guid todoItemId)
        {
            try
            {
                var todoItem = await _todoGenericService.GetByIdAsync(todoItemId, t => t.Comments);

                if (todoItem == null)
                {
                    return NotFound(new { message = "Todo item not found." });
                }

                var itemViewModel = new ItemResponseViewModel
                {
                    Id = todoItem.Id,
                    Title = todoItem.Title,
                    Comments = todoItem.Comments?.Select(comment => new CommentsViewModel
                    {
                        Id = comment.Id,
                        CommentText = comment.CommentText,
                        TodoItemId = comment.TodoItemId,
                        CreatedAt = comment.CreatedAt
                    }).ToList() ?? new List<CommentsViewModel>()
                };

                return View(itemViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the comments.");
                return View(new ItemResponseViewModel());
            }
        }

        // Get: Todo Item Form
        [HttpGet("TodoItem")]
        public IActionResult TodoItem()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var model = new ItemViewModel
                {
                    UserId = userId
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while preparing the form.");
                return View(new ItemViewModel());
            }
        }

        // POST: Add Todo Item
        [HttpPost("TodoItem")]
        public async Task<IActionResult> TodoItem(ItemViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var item = new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    IsComplete = model.IsComplete,
                    Priority = model.Priority,
                    UserId = model.UserId,
                };

                var success = await _todoGenericService.AddAsync(item);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add the todo item.");
                    return View(model);
                }

                return RedirectToAction("TodoItems");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
        }

        // POST: Delete Todo Item
        [HttpDelete]
        [Route("Home/DeleteTodoItem/{id:Guid}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                var success = await _todoGenericService.DeleteAsync(id);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Failed to delete the Todo item.");
                    return RedirectToAction("TodoItems");
                }

                return RedirectToAction("TodoItems");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while trying to delete the item.");
                return RedirectToAction("TodoItems");
            }
        }

        // GET: CommentController
        [HttpGet]
        [Route("Comment/GetComments")]
        public async Task<IActionResult> GetCommentsList()
        {
            try
            {
                var comments = await _commentService.GetAllAsync();
                return View(comments); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the comments.");
                return View(new List<TodoItemComment>());
            }
        }

        // POST: CommentController/Create
        [HttpPost]
        [Route("Comment/AddComment")]
        public async Task<IActionResult> AddComment(TodoItemCommentViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Comment))
                {
                    ModelState.AddModelError(string.Empty, "Comment should not be null or empty.");
                    return View("TodoItems", model);
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Create a new comment entity
                var comment = new TodoItemComment
                {
                    CommentText = model.Comment,
                    TodoItemId = model.TodoItemId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                var success = await _commentService.AddAsync(comment);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add the comment. Please try again later.");
                    return View("TodoItems", model);
                }

              
                return RedirectToAction("GetComments", new { todoItemId = model.TodoItemId });
            }
            catch (Exception ex)
            {
       
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
               
                return View("TodoItems", model);
            }
        }



        // POST: Add Todo Item
        [HttpPut("Home/UpdateItem/{Id}")]
        public async Task<IActionResult> UpdateItem(Guid Id , [FromBody] ItemViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("TodoItems", model);
                }

                var item = new TodoItem
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    IsComplete = model.IsComplete,
                    Priority = model.Priority,
                    UserId = model.UserId,
                };

                var success = await _todoGenericService.UpdateAsync(Id,item);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add the todo item.");
                    return View("TodoItems", model);
                }

                return RedirectToAction("TodoItems");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View("TodoItems", model);
            }
        }

    }
}
