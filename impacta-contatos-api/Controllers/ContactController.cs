using impacta_contatos_api.Helpers;
using impacta_contatos_api.Models;
using impacta_contatos_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Xml.XPath;


namespace impacta_contatos_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactServices _contactServices;

        public ContactController(ContactServices contactServices)
        {
            _contactServices = contactServices;
        }

        [HttpGet]
        public async Task<PagedResponse<Contact>> GetContacts(int pageNumber = 0, int pageSize = 10, string sortOrder = "ascending")
        {
            long count = await _contactServices.CountAsync();
            var contacts = await _contactServices.GetAsync(pageNumber, pageSize, sortOrder);

            var pagedResponse = new PagedResponse<Contact>
            {
                Count = count,
                Rows = contacts
            };

            return pagedResponse;
        }

        [HttpGet("find")]
        public async Task<PagedResponse<Contact>> GetContactsByField(string field, string value, int pageNumber = 0, int pageSize = 10, string sortOrder = "ascending")
        {
            long count = await _contactServices.CountByFieldAsync(field, value);
            var contacts = await _contactServices.FindByFieldAsync(field, value, pageNumber, pageSize, sortOrder);

            var pagedResponse = new PagedResponse<Contact>
            {
                Count = count,
                Rows = contacts
            };

            return pagedResponse;
        }

        [HttpGet("findByDate")]
        public async Task<PagedResponse<Contact>> GetContactsByDateAsync(DateTime dateTime, int pageNumber = 0, int pageSize = 10, string sortOrder = "ascending")
        {
            long count = await _contactServices.CountByDateAsync(dateTime);
            var contacts = await _contactServices.GetContactsByDateAsync(dateTime, pageNumber, pageSize, sortOrder);

            var pagedResponse = new PagedResponse<Contact>
            {
                Count = count,
                Rows = contacts
            };

            return pagedResponse;
        }

        [HttpGet("search")]
        public async Task<PagedResponse<Contact>> SearchContacts(string seachString, int pageNumber = 0, int pageSize = 10, string sortOrder = "ascending")
        {
            long count = await _contactServices.SearchCount(seachString);
            var contacts = await _contactServices.SearchContactsAsync(seachString, pageNumber, pageSize, sortOrder);

            var pagedResponse = new PagedResponse<Contact>
            {
                Count = count,
                Rows = contacts
            };

            return pagedResponse;
        }


        [HttpPost]
        public async Task<Contact> PostContact(Contact contact)
        {
            await _contactServices.CreateAsync(contact);

            return contact;
        }

        [HttpPost("mock")]
        public async Task<List<Contact>> MockContacts(int numberOfContacts = 100)
        {
            var contactGenerator = new ContactMockGenerator();
            var contacts = contactGenerator.GenerateLegalContacts(numberOfContacts);

            var createdContacts = new List<Contact>();
            foreach (var contact in contacts)
            {
                await _contactServices.CreateAsync(contact);
                createdContacts.Add(contact);
            }

            return createdContacts;
        }

        [HttpPut]
        public async Task<Contact> EditContact(Contact contact)
        {
            contact.UpdatedAt = DateTime.Now;
            await _contactServices.UpdateAsync(contact);

            return contact;
        }

        [HttpDelete]
        public async Task<Contact> RemoveContact(string contactId)
        {
            await _contactServices.RemoveAsync(contactId);

            var deletedContact = await _contactServices.FindOneAsync(contactId);
            return deletedContact;
        }
    }
}
