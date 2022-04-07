using SimpleEmailService.DataAccess.Entities;

namespace SimpleEmailService.Core.Models
{
    /// <summary>
    /// Collection of values that are used to filter down the list of total <see cref="Contact"/>s.
    /// </summary>
    public class ContactSearchValues
    {
        /// <summary>
        /// The partial value of name value, meant to filter the <see cref="Contact.Name"/> value by.
        /// </summary>
        public string NamePartial { get; set; }

        /// <summary>
        /// The earliest birthdate for a contact meant to be retrieved (comparison inclusive).
        /// </summary>
        public DateOnly? EarliestBirthDate { get; set; }

        /// <summary>
        /// The latest birthdate for a contact meant to be retrieved (comparison inclusive).
        /// </summary>
        public DateOnly? LatestBirthDate { get; set; }

        /// <summary>
        /// Utilizing the values of this <see cref="ContactSearchValues"/>' instance variables, generate a <see cref="Func{Contact, bool}<"/>.
        /// </summary>
        /// <returns><see cref="Func{Contact, bool}<"/></returns>
        public Func<Contact, bool> GenerateContactFilter()
        {
            Func<Contact, bool> filter = c => 
                (!string.IsNullOrEmpty(NamePartial) ? c.Name.Contains(NamePartial, StringComparison.OrdinalIgnoreCase) : true) &&
                (EarliestBirthDate != null ? c.BirthDate >= EarliestBirthDate : true) &&
                (LatestBirthDate != null ? c.BirthDate <= LatestBirthDate : true);

            return filter;
        }
    }
}
