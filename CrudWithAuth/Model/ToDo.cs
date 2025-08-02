using System.ComponentModel.DataAnnotations;

namespace CrudWithAuth.Model
{
    public class ToDo
    {
        public int Id { get; set; } = 0;
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool isDone { get; set; } = false;
        public int UserModelId { get; set; }
        public UserModel UserModel { get; set; }
    }
}
