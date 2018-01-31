using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleOidc.Data;
using SampleOidc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SampleOidc.Contacts.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsRepository _contactsRepository;
         private readonly ILogger<ContactsController> _logger;
        public ContactsController(ContactsRepository contactsRepository, ILogger<ContactsController> logger)
        {
            _contactsRepository = contactsRepository;
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<Contact> Get()
        {
            return _contactsRepository.Query().ToList();
        }

        [HttpPost]
        public Contact Post([FromBody]Contact contact)
        {
            var addedUser = _contactsRepository.Add(contact);

            return addedUser;
        }
    }
}
