using CrudWithAuth.Model.DTO;

namespace CrudWithAuth.Services
{
    public interface IToDoService
    {
        Task<ToDoDto?> GetToDoAsync(int Id);
        Task<List<ToDoDto>> GetToDosAsync();
        Task<ToDoDto> CreateToDo(ToDoDto NewToDo, int userId);
        Task<ToDoDto> UpdateToDoAsync(ToDoDto UpdateToDo);
        Task<bool?> DeleteToDoAsync(int Id);
    }
}
