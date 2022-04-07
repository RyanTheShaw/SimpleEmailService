using Microsoft.EntityFrameworkCore;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;

namespace SimpleEmailService.Core.Services
{
    public class ContactService
    {
        private readonly EmailDbContext _dbContext;

        public ContactService(EmailDbContext context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Retrieves a single <see cref="Contact"/> by it's <paramref name="id"/>, or null if no match is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="Contact?"/> matching the supplied id</returns>
        public async Task<Contact?> GetContact(long id)
        {
            var result = await _dbContext.Contacts.Include(c => c.Emails).FirstOrDefaultAsync(c => c.Id == id);

            return result;
        }

        /// <summary>
        /// Returns a list of contacts contained in the <see cref="EmailDbContext"/>, optionally filtered by the supplied <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">Collection of values meant to filter the list of <see cref="Contact"/>s</param>
        /// <returns><see cref="Task{IEnumerable{Contact}}"/></returns>
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

            // Updates made property by property to keep change tracking.
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
