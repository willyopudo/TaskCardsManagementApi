namespace LogiceaCardsManagementApp2.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int Role { get; set; }
    }

    public enum UserRoles
    {
        Admin = 0,
        Member = 1
    }
}
