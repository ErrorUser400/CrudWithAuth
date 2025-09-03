namespace CrudWithAuth.Entitites
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PassWordHash { get; set; } = string.Empty;
        public UserRoles Roles { get; set; } = UserRoles.None;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<ToDo>? ToDos { get; set; }
    }

    public enum UserRoles
    {
        None,
        User,
        Admin,
    }
}
