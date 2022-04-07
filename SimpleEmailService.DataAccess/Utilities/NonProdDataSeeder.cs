using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;

namespace SimpleEmailService.DataAccess.Utilities
{
    public static class NonProdDataSeeder
    {
        public static async Task Seed(EmailDbContext context)
        {
            // Early out in the case of us already having data in these tables.
            if (context.Emails.Any() || context.Contacts.Any())
                return;

            context.Database.EnsureCreated();

            List<Email> RyansEmails = new List<Email>()
                {
                    new Email()
                    {
                        Id = 1,
                        Address = "RyanEmail1@example.com",
                        IsPrimary = true,
                    },
                    new Email()
                    {
                        Id = 2,
                        Address = "RyanEmail2@example.com",
                        IsPrimary = false,
                    },
                    new Email()
                    {
                        Id = 3,
                        Address = "RyanEmail3@example.com",
                        IsPrimary = false,
                    }
                };

            List<Email> JoesEmails = new List<Email>()
                {
                    new Email()
                    {
                        Id = 4,
                        Address = "JoeEmail1@example.com",
                        IsPrimary = false,
                    },
                    new Email()
                    {
                        Id = 5,
                        Address = "JoeEmail2@example.com",
                        IsPrimary = false,
                    },
                    new Email()
                    {
                        Id = 6,
                        Address = "JoeEmail3@example.com",
                        IsPrimary = true,
                    }
                };

            await context.Contacts.AddAsync(new Contact()
            {
                Id = 1,
                Name = "Ryan Shaw",
                BirthDate = new DateOnly(1994, 9, 28),
                Emails = RyansEmails
            });

            await context.Contacts.AddAsync(new Contact()
            {
                Id = 2,
                Name = "Joe Shmoe",
                BirthDate = new DateOnly(1984, 4, 14),
                Emails = JoesEmails
            });

            context.SaveChanges();
        }
    }
}
