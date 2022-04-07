using Microsoft.EntityFrameworkCore;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;
using SimpleEmailService.DataAccess.Utilities;

namespace SimpleEmailService.Core
{
    public class ContactService
    {
        private readonly EmailDbContext _dbContext;

        public ContactService(EmailDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Contact?> GetContact(long id)
        {
            var result = await _dbContext.Contacts.Include(c => c.Emails).FirstOrDefaultAsync(c => c.Id == id);

            return result;
        }

        public async Task<IEnumerable<Contact>> GetContacts(Func<Contact, bool> filter = null)
        {
            if (filter == null)
                filter = u => true;

            var result = _dbContext.Contacts.Include(c => c.Emails).Where(filter);

            return result;
        }

        public async Task<bool> CreateContact(Contact contact)
        {
            // Todo, add validation...
            if (!await IsContactValid(contact))
                return false;

            await _dbContext.Contacts.AddAsync(contact);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task UpdateContact(Contact contact)
        {
            var dbContact = await _dbContext.Contacts.Include(c => c.Emails).FirstOrDefaultAsync(c => c.Id == contact.Id);
            if(dbContact == null || !await IsContactValid(contact))
                return;

            //TODO: If there is an expectation for these shapes changing over time, maybe use memberwise clone / reflection?
            dbContact.Name = contact.Name;
            dbContact.BirthDate = contact.BirthDate;
            dbContact.Emails = contact.Emails;

            await _dbContext.SaveChangesAsync();
        }

        private async Task<bool> IsContactValid(Contact contact)
        {
            return contact.Emails.Where(e => e.IsPrimary).Count() == 1;
        }
    }
}
