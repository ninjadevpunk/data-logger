namespace Data_Logger_1._3.Models.App_Models
{
    public class UserSettings
    {
        public int Id { get; set; } = 0;
        public string ProfilePic { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsCompanyEmployee { get; set; } = false;
        public string? CompanyName { get; set; } = null;
        public string? CompanyAddress { get; set; } = null;
    }
}
