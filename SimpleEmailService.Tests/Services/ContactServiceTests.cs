using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SimpleEmailService.Core;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

//[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace SimpleEmailService.Tests.Services
{
    public class ContactServiceTests
    {
        #region GetContact / GetContacts Tests

        [Fact]
        public async Task GetContacts_WithOneContactStored_RetrievesOneContact()
        {
            // Arrange
            var contact = GenerateMockContactRyan();

            using (var context = GenerateEmailDbContext("Contact test 6"))
            {
                context.Contacts.Add(contact);
                context.SaveChanges();

                ContactService service = new ContactService(context);

                // Act
                var contacts = await service.GetContacts();

                // Assert
                Assert.Single(contacts);
                Assert.Equal(contact, contacts.First());
            }
        }

        [Fact]
        public async Task GetContacts_WithTwoContactStored_RetrievesTwoContacts()
        {
            // Arrange
            var contact = GenerateMockContactRyan();
            var contact2 = new Contact()
            {
                Name = "Weird Al",
                BirthDate = new DateOnly(1959, 10, 23),
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Address = "weirdalemail@example.com",
                        IsPrimary = true
                    }
                }
            };

            using (var context = GenerateEmailDbContext("Contact test 7"))
            {
                context.Contacts.Add(contact);
                context.Contacts.Add(contact2);
                context.SaveChanges();

                ContactService service = new ContactService(context);

                // Act
                var contacts = await service.GetContacts();

                // Assert
                Assert.Equal(2, contacts.Count());
                Assert.Equal(contact, contacts.FirstOrDefault(c => c.Id == 1));
                Assert.Equal(contact2, contacts.FirstOrDefault(c => c.Id == 2));
            }
        }

        [Fact]
        public async Task GetContacts_WithZeroContactStored_RetrievesZeroContacts()
        {
            // Arrange
            using (var context = GenerateEmailDbContext("Contact test 8"))
            {
                ContactService service = new ContactService(context);

                // Act
                var contacts = await service.GetContacts();

                // Assert
                Assert.Empty(contacts);
            }
        }

        [Fact]
        public async Task GetContact_WithTwoContactStored_RetrievesCorrectContacts()
        {
            // Arrange
            var contact1 = GenerateMockContactRyan();
            var contact2 = new Contact()
            {
                Name = "Weird Al",
                BirthDate = new DateOnly(1959, 10, 23),
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Address = "weirdalemail@example.com",
                        IsPrimary = true
                    }
                }
            };

            using (var context = GenerateEmailDbContext("Contact test 9"))
            {
                context.Contacts.Add(contact1);
                context.Contacts.Add(contact2);
                context.SaveChanges();

                ContactService service = new ContactService(context);

                // Act
                var retrievedContact1 = await service.GetContact(1);
                var retrievedContact2 = await service.GetContact(2);

                // Assert
                Assert.Equal(contact1, retrievedContact1);
                Assert.NotEqual(contact2, retrievedContact1);

                Assert.Equal(contact2, retrievedContact2);
                Assert.NotEqual(contact1, retrievedContact2);
            }
        }

        #endregion

        #region CreateContact Tests

        [Fact]
        public async Task CreateContact_WithValidFields_SuccessfullyStored()
        {
            // Arrange
            var contact = GenerateMockContactRyan();

            using (var context = GenerateEmailDbContext("Contact test 1"))
            {
                ContactService service = new ContactService(context);

                // Act
                var isSuccessful = await service.CreateContact(contact);

                // Assert
                Assert.True(isSuccessful);
                Assert.Equal(1, context.Contacts.Count());
            }
        }

        [Fact]
        public async Task CreateContact_WithNoEmails_SuccessfullyStores()
        {
            // Arrange
            var contact = GenerateMockContactRyan();

            contact.Emails = new List<Email>();

            using (var context = GenerateEmailDbContext("Contact test 2"))
            {
                ContactService service = new ContactService(context);

                // Act
                var isSuccessful = await service.CreateContact(contact);

                // Assert
                Assert.True(isSuccessful);
                Assert.Equal(1, context.Contacts.Count());
            }
        }

        [Fact]
        public async Task CreateContact_WithTwoPrimaryEmails_FailsToStore()
        {
            // Arrange
            var contact = GenerateMockContactRyan();

            contact.Emails = new List<Email>()
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
                        IsPrimary = true,
                    },
                    new Email()
                    {
                        Id = 3,
                        Address = "RyanEmail3@example.com",
                        IsPrimary = false,
                    }
                };

            using (var context = GenerateEmailDbContext("Contact test 3"))
            {
                ContactService service = new ContactService(context);

                // Act
                var isSuccessful = await service.CreateContact(contact);

                // Assert
                Assert.False(isSuccessful);
                Assert.Equal(0, context.Contacts.Count());
            }
        }

        #endregion

        #region UpdateContact Tests

        [Fact]
        public async Task UpdateContact_WithExistingId_SuccessfullyUpdates()
        {
            // Arrange
            var baseContact = GenerateMockContactRyan();

            using (var context = GenerateEmailDbContext("Contact test 4"))
            {
                ContactService service = new ContactService(context);

                await service.CreateContact(baseContact);
                var storedContact = await service.GetContact(1);

                storedContact.Name = "Ryan Shaw - Updated";
                storedContact.BirthDate = new DateOnly(2004, 4, 10);
                storedContact.Emails = new List<Email>() { new()
                    {
                        Address = "ThisIsANewAddress@example.com",
                        IsPrimary = true,
                    },
                    new() {
                        Address = "ThisIsAnotherNewAddress@example.com",
                        IsPrimary= false,
                    }
                };

                // Act
                var isSuccessful = await service.UpdateContact(storedContact);
                var retrievedContact = context.Contacts.First();

                // Assert
                Assert.True(isSuccessful);
                Assert.Equal(1, context.Contacts.Count());
                Assert.Equal(storedContact, retrievedContact);
            }
        }

        [Fact]
        public async Task UpdateContact_WithoutExistingId_FailsToUpdate()
        {
            // Arrange
            var baseContact = GenerateMockContactRyan();

            using (var context = GenerateEmailDbContext("Contact test 5"))
            {
                ContactService service = new ContactService(context);

                await service.CreateContact(baseContact);
                var storedContact = await service.GetContact(1);
                var newContacts = new Contact()
                {
                    Id = 9999,
                    Name = "Ryan Shaw - Updated",
                    BirthDate = new DateOnly(2004, 4, 10),
                    Emails = new List<Email>() { new()
                        {
                            Address = "ThisIsANewAddress@example.com",
                            IsPrimary = true,
                        },
                        new() {
                            Address = "ThisIsAnotherNewAddress@example.com",
                            IsPrimary= false,
                        }
                    }
                };

                // Act
                var isSuccessful = await service.UpdateContact(newContacts);
                var originalContact = context.Contacts.First();

                // Assert
                Assert.False(isSuccessful);
                Assert.Equal(1, context.Contacts.Count());
                Assert.Equal(baseContact, originalContact);
            }
        }

        #endregion

        #region Private helper methods

        private EmailDbContext GenerateEmailDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<EmailDbContext>()
                   .UseInMemoryDatabase(databaseName)
                   .Options;

            return new EmailDbContext(options);
        }

        private Contact GenerateMockContactRyan()
        {
            Contact contact = new Contact()
            {
                Name = "Ryan Shaw",
                BirthDate = new DateOnly(1994, 9, 28),
                Emails = new List<Email>()
                {
                    new Email()
                    {
                        Address = "RyanEmail1@example.com",
                        IsPrimary = true,
                    },
                    new Email()
                    {
                        Address = "RyanEmail2@example.com",
                        IsPrimary = false,
                    },
                    new Email()
                    {
                        Address = "RyanEmail3@example.com",
                        IsPrimary = false,
                    }
                }
            };

            return contact;
        }

        #endregion
    }
}
