using Microsoft.AspNetCore.Mvc;
using SimpleEmailService.Core;
using SimpleEmailService.DataAccess.Entities;
using System.Net;

namespace SimpleEmailService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly ContactService _contactService;

        public ContactController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await _contactService.GetContacts();
        }

        //[HttpGet("{contactId}")]
        //public IEnumerable<Email> Get(int contactId)
        //{
        //    Func<Contact, bool> filter = e => e?.Contact.Id == contactId;

        //    return _contactService.GetContact(filter);
        //}

        [HttpPost]
        public async Task<HttpStatusCode> Post(Contact contact)
        {
            if(await _contactService.CreateContact(contact))
            {
                return HttpStatusCode.Created;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        public async Task<Contact> Put(Contact contact)
        {
            await _contactService.UpdateContact(contact);

            return await _contactService.GetContact(contact.Id);
        }

        //[HttpPatch]
        //public async Task<HttpStatusCode> Patch(Contact contact)
        //{
        //    await _contactService.UpdateContact(contact);

        //    return HttpStatusCode.Created;
        //}
    }
}
