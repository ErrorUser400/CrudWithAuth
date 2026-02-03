namespace CrudWithAuth.Entitites
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsDone { get; set; } = false;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
