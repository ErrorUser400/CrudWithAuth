using CrudWithAuth.Entitites;

namespace CrudWithAuth.Model.DTO
{
    public class ToDoDto
    {
        //code made a fitt if i didn't add it. need default constructor
        public ToDoDto()
        {
            this.Id = 0;
            this.Title = string.Empty;
            this.IsDone = false;
        }
        //map todo to a tododto to reduce what is sent out
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
