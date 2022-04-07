using Microsoft.AspNetCore.Mvc;
using SimpleEmailService.Core.Models;
using SimpleEmailService.Core.Services;
using SimpleEmailService.DataAccess.Entities;

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

        [Route("Search")]
        [HttpPost]
        public async Task<IEnumerable<Contact>> Search(ContactSearchValues contactSearchValues)
        {
            return await _contactService.GetContacts(contactSearchValues.GenerateContactFilter());
        }

        [HttpPost]
        public async Task<ActionResult<Contact?>> Post(Contact contact)
        {
            if(await _contactService.CreateContact(contact))
            {
                return Ok(contact);
            }
            else
            {
                return BadRequest("Failed to create contact.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Contact?>> Put(Contact contact)
        {
            if(await _contactService.UpdateContact(contact))
            {
                return Ok(await _contactService.GetContact(contact.Id));
            }
            else
            {
                return BadRequest("Failed to update contact.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact?>> Delete(long id)
        {
            if (await _contactService.DeleteContact(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to delete contact.");
            }
        }
    }
}
