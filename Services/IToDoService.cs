using CrudWithAuth.Entitites;
using CrudWithAuth.Model.DTO;

namespace CrudWithAuth.Services
{
    public interface IToDoService
    {
        Task<ToDo?> GetToDoAsync(int Id);
        Task<List<ToDo>> GetToDosAsync();
        Task<ToDo> CreateToDo(ToDoDto NewToDo, int userId);
        Task<ToDo> UpdateToDoAsync(ToDoDto UpdateToDo);
        Task<bool?> DeleteToDoAsync(int Id);
    }
}
