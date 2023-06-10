namespace Assignment.Contracts.DTO
{
    public class CreateUserDTO
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public string? Provider { get; set; }
        public string? IdToken { get; set; }
    }
}