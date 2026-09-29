namespace Core.DTOs.Viewer
{
    public record CodingViewDTO : ViewDTO
    {
        public string Bugs { get; set; } = string.Empty;
        public string ApplicationOpened { get; set; } = "Unsuccessful Launch";
    }
}
