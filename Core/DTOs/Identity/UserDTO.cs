namespace Core.DTOs.Identity
{
    public record UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string ProfilePic { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmployee { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyLogo { get; set; }
    }
}
