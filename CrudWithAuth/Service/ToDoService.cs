using CrudWithAuth.Data;
using CrudWithAuth.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrudWithAuth.Service
{
    public class ToDoService : ITodoService
    {
        private readonly AppDbContext _context;
        private ILogger<ToDoService> _logger;
        public ToDoService(AppDbContext appDbContext, ILogger<ToDoService> logger)
        {
            _context = appDbContext;
            _logger = logger;
        }
        public async Task<ToDo> CreateToDo(ToDo addToDo)
        {
            try
            {
                await _context.toDos.AddAsync(addToDo);
                await _context.SaveChangesAsync();
            }
            catch (DBConcurrencyException ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            return addToDo;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.toDos.FindAsync(id);
            if (result == null)
            {
                return false;
            }

            _context.Remove(result);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ToDo> GetTodo(int id)
        {
            var result = await _context.toDos.FindAsync(id);
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<List<ToDo>> GetToDos()
        {
            var results = await _context.toDos.ToListAsync<ToDo>() ?? [];
            return results;
        }

        public async Task<ToDo> UpdateToDo(int id, ToDo updateToDo)
        {

            if (id != updateToDo.Id)
            {
                return null;
            }

            _context.Entry(updateToDo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.toDos.Any(e => e.Id == id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return updateToDo;
        }
    }
}
