namespace CrudWithAuth.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAdress { get; set; }
        public DateTime JoinedAt { get; set; }
        public List<ToDo> toDos { get; set; }
    }
}
