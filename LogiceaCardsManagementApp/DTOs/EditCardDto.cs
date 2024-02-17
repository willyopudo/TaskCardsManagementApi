using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LogiceaCardsManagementApp2.DTOs
{
    public class EditCardDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Card name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^#?([a-f0-9]{6})$", ErrorMessage = "Color value was incorrect. Should be in Hex format #000000.")]
        public string Color { get; set; }
        public string Status { get; set; }
    }
}
