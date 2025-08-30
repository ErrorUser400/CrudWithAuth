namespace CrudWithAuth.Model.DTO
{
    public class RefreshTokenRequestDto
    {
        public int Id { get; set; }
        public required string RefreshToken { get; set; }
    }
}
