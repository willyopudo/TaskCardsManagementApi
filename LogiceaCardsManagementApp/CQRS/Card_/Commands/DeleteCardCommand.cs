using LogiceaCardsManagementApp2.Data;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LogiceaCardsManagementApp2.CQRS.Card_.Commands
{
    public class DeleteCardCommand : IRequest<int>
    {
        public int Id { get; set; }
        public User loggedInUser { get; set; }

        public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, int>
        {
            private ApplicationDbContext _context;
            private ILogger<DeleteCardCommand> _logger;

            public DeleteCardCommandHandler(ApplicationDbContext context, ILogger<DeleteCardCommand> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<int> Handle(DeleteCardCommand command, CancellationToken cancellationToken)
            {
                Card card = _context.cards.Find(command.Id);
                if (card != null)
                {
                    if (command.loggedInUser.Role == 0)
                    {
                        _context.cards.Remove(card);
                    }
                    else
                    {
                        try
                        {
                            if (card.CreatedBy == command.loggedInUser.Id)
                            {
                                _context.cards.Remove(card);
                            }
                            else
                                return 2;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                    }

                    await _context.SaveChangesAsync();
                    return 1;
                }
                return 3;
            }
        }
    }
}
