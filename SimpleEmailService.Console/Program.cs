// Note: this console application is extremely barebones...
//          Given more time I would look to make it more interactive and robust
using Microsoft.EntityFrameworkCore;
using SimpleEmailService.Core.Services;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Entities;

var options = new DbContextOptionsBuilder<EmailDbContext>()
                   .UseInMemoryDatabase("EmailDb")
                   .Options;

EmailDbContext context = new EmailDbContext(options);
ContactService _service = new ContactService(context);


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Welcome to the simple email service console application!");
Console.WriteLine("This app serves as a quick demo of some of the business logic found in the SimpleEmailService.Core projet.");
Console.WriteLine("At the end of each CRUD operation, the state of our contact will be displayed to the console to allow for you to see it.");
Console.WriteLine("Consider debugging the application if you'd like a view into the state of all variables at any time in the process.\n");

Console.WriteLine("Press enter to add Mr. Console-App as a contact");
Console.ReadLine();
var contact = GenerateContactRecord();
await _service.CreateContact(contact);
Console.WriteLine(await GenerateContactStringAsync(contact));

Console.WriteLine("Press enter to update Mr. Console-App's name to `Mrs. Console-App`");
Console.ReadLine();
contact.Name = "Mrs. Console-App";
await _service.UpdateContact(contact);
Console.WriteLine(await GenerateContactStringAsync(contact));

Console.WriteLine("Press enter to delete Mrs. Console-App's in-memory record");
Console.ReadLine();
await _service.DeleteContact(contact.Id);

#region helper methods

Contact GenerateContactRecord()
{
    return new Contact()
    {
        Name = "Mr. Console-App",
        BirthDate = new DateOnly(1900, 1, 1),
        Emails = new List<Email>()
        {
            new Email()
            {
                Address = "noreply@consoleapp.com",
                IsPrimary = true
            },
            new Email()
            {
                Address = "noreply2@consoleapp.com",
                IsPrimary = false
            }
        }
    };
}

async Task<string> GenerateContactStringAsync(Contact contact)
{
    return $"{await _service.GetContact(contact.Id)}\n";
}

#endregion
