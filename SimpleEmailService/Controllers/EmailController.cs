using Microsoft.AspNetCore.Mvc;
using SimpleEmailService.Core;
using SimpleEmailService.DataAccess.Entities;
using System.Net;

namespace SimpleEmailService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public IEnumerable<Email> Get()
        {
            return _emailService.GetEmails();
        }

        [HttpGet("{contactId}")]
        public IEnumerable<Email> Get(int contactId)
        {
            //Func<Email, bool> filter = e => e?.Contact.Id == contactId;

            return _emailService.GetEmails();
        }

        //[HttpPost]
        //public async Task<HttpStatusCode> Post(Email email)
        //{
        //    await _emailService.CreateEmail(email);

        //    return HttpStatusCode.Created;
        //}

        [HttpPost(Name = "MockRecords")]
        public async Task MockRecords()
        {
            await _emailService.MockRecords();
        }
    }
}
