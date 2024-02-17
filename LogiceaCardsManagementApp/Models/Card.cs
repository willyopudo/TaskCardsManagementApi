using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogiceaCardsManagementApp2.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Card name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^#?([a-f0-9]{6})$", ErrorMessage = "Color value was incorrect. Should be in Hex format #000000.")]
        public string Color { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public enum CardStatus
    {
        ToDo = 0,
        InProgress = 1,
        Done = 2
    }
}
