using CrudWithAuth.Model;
using CrudWithAuth.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudWithAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public ToDoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ToDo>> CreateToDo(ToDoDTO newToDo)
        {
            var userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _todoService.CreateToDo(new ToDo
            {
                Id = 0,
                Title = newToDo.Title,
                Description = newToDo.Description,
                isDone = newToDo.isDone,
                UserModelId = userId
            });

            if (result == null)
            {
                return BadRequest("Couldn't add object");
            }

            return Ok(result);
        }

        [HttpGet("ToDos")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetAllToDoes()
        {
            return Ok(await _todoService.GetToDos());
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
            var result = await _todoService.GetTodo(id);
            if (result == null)
            {
                return NotFound("can't find object");
            }

            return Ok(result);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult<ToDo>> UpdateToDo(int id, ToDo updateToDo)
        {
            var result = await _todoService.UpdateToDo(id, updateToDo);
            if (result == null)
            {
                return BadRequest("Could't update object");
            }

            return Ok(updateToDo);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<bool>> DeleteToDo(int id)
        {
            bool result = await _todoService.Delete(id);

            if (result == false)
            {
                return BadRequest("Could't delete object");
            }

            return Ok(result);
        }
    }
}
