using LogiceaCardsManagementApp2.CQRS.Card_.Commands;
using LogiceaCardsManagementApp2.Data;
using LogiceaCardsManagementApp2.DTOs;
using LogiceaCardsManagementApp2.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LogiceaCardsManagementApp2.CQRS.User_.Queries
{
    public class FindUserByEmailQuery : IRequest<User>
    {
        public string email { get; set; }
        public string Password { get; set; }

        public class FindUserByEmailQuerHandler : IRequestHandler<FindUserByEmailQuery, User>
        {
            private ApplicationDbContext _context;

            public FindUserByEmailQuerHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<User> Handle(FindUserByEmailQuery query, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(query.Password))
                    return await _context.users.Where(s => s.Email == query.email).FirstOrDefaultAsync();
                else
                    return await _context.users.Where(s => s.Email == query.email && s.Password == query.Password).FirstOrDefaultAsync();
                
            }
        }
    }
}
