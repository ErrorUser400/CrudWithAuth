using CrudWithAuth.Model.DTO;
using CrudWithAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [Authorize]
        [HttpGet("/todo{Id}")]
        public async Task<ActionResult<ToDoDto>> GetToDo(int Id)
        {
            var result = await _toDoService.GetToDoAsync(Id);

            if (result is null)
            {
                return BadRequest("Not found");
            }

            return Ok(result);
        }

        [HttpGet("/todos")]
        public async Task<ActionResult<List<ToDoDto>>> GetToDos()
        {
            return Ok(await _toDoService.GetToDosAsync());
        }

        [Authorize]
        [HttpPost("/create")]
        public async Task<ActionResult<ToDoDto>> CreateToDo(ToDoDto request)
        {
            //var userId = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _toDoService.CreateToDo(request, userId);

            return Ok(result);
        }

        [Authorize]
        [HttpPatch("/update")]
        public async Task<ActionResult<ToDoDto>> UpdateTodo(ToDoDto request)
        {
            var result = await _toDoService.UpdateToDoAsync(request);

            if (result is null)
            {
                return BadRequest("Not found");
            }

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("/delete{Id}")]
        public async Task<ActionResult<string>> DeleteToDo(int Id)
        {
            var result = await _toDoService.DeleteToDoAsync(Id);

            if (result is null)
            {
                return BadRequest("Not found");
            }

            return Ok("To Do Deleted");
        }
    }
}
