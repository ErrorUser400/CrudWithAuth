using CrudWithAuth.Entitites;

namespace CrudWithAuth.Model.DTO
{
    public class ToDoDto
    {
        public ToDoDto()
        {
            this.Id = 0;
            this.Title = string.Empty;
            this.IsDone = false;
        }
        public ToDoDto(ToDo toDo)
        {
            Id = toDo.Id;
            Title = toDo.Title;
            IsDone = toDo.IsDone;
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
    }
}
