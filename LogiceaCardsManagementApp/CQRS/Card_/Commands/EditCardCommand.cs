using LogiceaCardsManagementApp2.Data;
using LogiceaCardsManagementApp2.DTOs;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LogiceaCardsManagementApp2.CQRS.Card_.Commands
{
    public class EditCardCommand : IRequest<Card>
    {
        public EditCardDto cardDto { get; set; }
        public User loggedInUer {  get; set; }

        public class EditCardCommandHandler : IRequestHandler<EditCardCommand, Card>
        {
            private ApplicationDbContext _context;
            private ILogger<DeleteCardCommand> _logger;

            public EditCardCommandHandler(ApplicationDbContext context, ILogger<DeleteCardCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Card> Handle(EditCardCommand command, CancellationToken cancellationToken)
            {
                Card card = _context.cards.Find(command.cardDto.Id);
                if (card != null)
                {
                    card.Name = command.cardDto.Name;
                    card.Description = command.cardDto.Description;
                    card.Color = command.cardDto.Color;

                    //Let's check if user passed existing card status
                    try
                    {
                        card.Status = (int)Enum.Parse<CardStatus>(command.cardDto.Status);
                    }
                    catch(Exception ex) {
                        _logger.LogError(ex.ToString());

                        //Hack for returning appropriate message to user in Controller
                        return new Card() { Id = -1};
                    }

                    if (command.loggedInUer.Role == 0)
                    {

                        _context.Entry(card).State = EntityState.Modified;
                    }
                    else
                    {
                        try
                        {
                            if (card.CreatedBy == command.loggedInUer.Id)
                            {
                                _context.Entry(card).State = EntityState.Modified;
                            }
                            else
                                return new Card();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                    }
                }
                
                await _context.SaveChangesAsync();
                return card;
            }
        }
    }
}
