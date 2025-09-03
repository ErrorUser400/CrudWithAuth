namespace CrudWithAuth.Model.DTO
{
    public class ToDoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
