namespace Backend.Models
{
    public class ContactDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Category {  get; set; } = string.Empty;
        public string Subcategory { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
    }
}
