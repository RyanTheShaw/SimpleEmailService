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

        /// <summary>
        /// Attempts to store the provided <see cref="Contact"/> in the <see cref="EmailDbContext"/>
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be created.</param>
        /// <returns>True if successful, false if failure.</returns>
        public async Task<bool> CreateContact(Contact contact)
        {
            // Todo, add validation...
            if (!await IsContactValid(contact))
                return false;

            await _dbContext.Contacts.AddAsync(contact);

            return await SaveChangesWithExceptionHandling();
        }

        /// <summary>
        /// Attempts to update the provided <see cref="Contact"/> by Id in the <see cref="EmailDbContext"/>
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be updated.</param>
        /// <returns>True if successful, false if failure.</returns>
        public async Task<bool> UpdateContact(Contact contact)
        {
            var dbContact = await _dbContext.Contacts.Include(c => c.Emails).FirstOrDefaultAsync(c => c.Id == contact.Id);
            if(dbContact == null || !await IsContactValid(contact))
                return false;

            dbContact.Name = contact.Name;
            dbContact.BirthDate = contact.BirthDate;
            dbContact.Emails = contact.Emails;

            return await SaveChangesWithExceptionHandling();
        }

        /// <summary>
        /// Attempts to delete the provided <see cref="Contact"/> by Id in the <see cref="EmailDbContext"/>
        /// </summary>
        /// <param name="contact"><see cref="Contact"/> to be deleted.</param>
        /// <returns>True if successful, false if failure.</returns>
        public async Task<bool> DeleteContact(long id)
        {
            var dbContact = await _dbContext.Contacts.Include(c => c.Emails).FirstOrDefaultAsync(c => c.Id == id);
            if (dbContact == null)
                return false;

            _dbContext.Contacts.Remove(dbContact);

            return await SaveChangesWithExceptionHandling();
        }

        #region Private helper methods

        private async Task<bool> IsContactValid(Contact contact)
        {
            return contact.Emails == null || 
                !contact.Emails.Any() || 
                contact.Emails.Where(e => e.IsPrimary).Count() == 1;
        }
        
        private async Task<bool> SaveChangesWithExceptionHandling()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbUpdateConcurrencyException || ex is OperationCanceledException)
            {
                // Ideally add some logging here, but returning failure is good enough for now.
                return false;
            }
        }

        #endregion
    }
}
