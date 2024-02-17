using LogiceaCardsManagementApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace LogiceaCardsManagementApp2.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw = "")
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                SeedDB(context, testUserPw);
            }
        }

        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.users.Any())
            {
                return;   // DB has been seeded
            }

            context.users.AddRange(
                new User
                {                  
                    Password = "123456",
                    Role = 0,
                    Email = "debra@logicea.com"
                },
                new User
                {
                    Password = "12345",
                    Role = 1,
                    Email = "user1@logicea.com"
                },
                
                new User
                {
                    Password = "96354",
                    Role = 1,
                    Email = "user2@logicea.com"
                }
             );
            context.SaveChanges();
        }

    }
}
