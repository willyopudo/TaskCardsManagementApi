using LogiceaCardsManagementApp2.Data;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LogiceaCardsManagementApp2.CQRS.Card_.Commands
{
    public class CreateCardCommand : IRequest<Card>
    {
        [Required(ErrorMessage = "Card name is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^#([a-f0-9]{6})$", ErrorMessage = "Color value was incorrect. Should be in Hex format #000000.")]
        public string Color { get; set; }

        [JsonIgnore]
        public int CreatedBy { get; set; }

        public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, Card>
        {
            private ApplicationDbContext _context;
            private ILogger<DeleteCardCommand> _logger;

            public CreateCardCommandHandler(ApplicationDbContext context, ILogger<DeleteCardCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Card> Handle(CreateCardCommand command, CancellationToken cancellationToken)
            {
                var card = new Card();
                card.Name = command.Name;
                card.Description = command.Description;
                card.Color = command.Color;         
                card.CreatedBy = command.CreatedBy;
                card.CreatedDate = DateTime.Now;
                card.Status = 0;

                _context.cards.Add(card);
                await _context.SaveChangesAsync();
                return card;
            }
        }
    }
}
