using LogiceaCardsManagementApp2.CQRS.User_.Queries;
using LogiceaCardsManagementApp2.Data;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LogiceaCardsManagementApp2.CQRS.Card_.Queries
{
    public class FindCardsQuery : IRequest<List<Card>>
    {
        public int Id { get; set; }
        public User loggedInUser { get; set; }

        public class FindCardsQueryHandler : IRequestHandler<FindCardsQuery, List<Card>>
        {
            private ApplicationDbContext _context;

            public FindCardsQueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<Card>> Handle(FindCardsQuery query, CancellationToken cancellationToken)
            {
                List<Card> cards = new List<Card>();
                if (query.loggedInUser.Role == 0 && query.Id < 1)
                   cards =  await _context.cards.ToListAsync();

                else if(query.loggedInUser.Role == 0 && query.Id > 0)
                   cards.Add( await _context.cards.FindAsync(query.Id));

                else if(query.loggedInUser.Role == 1 && query.Id < 1)
                    cards = await _context.cards.Where(c => c.CreatedBy == query.loggedInUser.Id).ToListAsync();

                else
                    cards = await _context.cards.Where(c => c.Id == query.Id && c.CreatedBy == query.loggedInUser.Id).ToListAsync();

                return cards;            
            }
        }
    }
}
