using Microsoft.EntityFrameworkCore;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;
using SimpleEmailService.DataAccess.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEmailService.Core
{
    public class EmailService
    {
        private readonly EmailDbContext _dbContext;

        public EmailService(EmailDbContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<Email> GetEmails(Func<Email, bool> filter = null)
        {
            if (filter == null)
                filter = u => true;

            var result = _dbContext.Emails.Where(filter);

            return result;
        }

        public async Task CreateEmail(Email email)
        {
            // Todo, add validation...

            await _dbContext.Emails.AddAsync(email);
        }

        public async Task MockRecords()
        {
            await NonProdDataSeeder.Seed(_dbContext);
        }
    }
}
