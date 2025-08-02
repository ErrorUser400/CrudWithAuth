using CrudWithAuth.Model;

namespace CrudWithAuth.Service
{
    public interface ITodoService
    {
        Task<ToDo> GetTodo(int id);
        Task<List<ToDo>> GetToDos();
        Task<ToDo> CreateToDo(ToDo addToDo);
        Task<ToDo> UpdateToDo(int id, ToDo updateToDo);
        Task<bool> Delete(int id);
    }
}
