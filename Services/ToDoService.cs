using CrudWithAuth.Data;
using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace CrudWithAuth.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;

        public ToDoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToDo> CreateToDo(ToDoDto NewToDo, int userId)
        {
            var newtodo = new ToDo { Title = NewToDo.Title, CreatedDate = NewToDo.CreatedDate, IsDone = NewToDo.IsDone, UserId = userId };

            _context.toDos.Add(newtodo);
            await _context.SaveChangesAsync();

            return newtodo;
        }

        public async Task<bool?> DeleteToDoAsync(int Id)
        {
            var todo = await _context.toDos.FindAsync(Id);

            if (todo is null)
            {
                return null;
            }

            _context.toDos.Remove(todo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ToDo?> GetToDoAsync(int Id)
        {
            var todo = await _context.toDos.FirstOrDefaultAsync(t => t.Id == Id);
            if (todo is null)
            {
                return null;
            }

            return todo;
        }

        public async Task<List<ToDo>> GetToDosAsync()
        {
            var todo = await _context.toDos.ToListAsync();

            if (todo is null || todo.Count == 0)
            {
                return Enumerable.Empty<ToDo>().ToList();
            }

            return todo;
        }

        public async Task<ToDo?> UpdateToDoAsync(ToDoDto UpdateToDo)
        {
            var todo = await _context.toDos.FirstOrDefaultAsync(t => t.Id == UpdateToDo.Id);

            if (todo is null)
            {
                return null;
            }

            todo.IsDone = UpdateToDo.IsDone;
            todo.Title = UpdateToDo.Title;

            await _context.SaveChangesAsync();

            return todo;
        }
    }
}
