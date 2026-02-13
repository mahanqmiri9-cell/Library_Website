namespace LibraryWebsite.Service.DTOs
{
    public class UserGetByIdDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
