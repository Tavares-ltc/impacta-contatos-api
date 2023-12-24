using impacta_contatos_api.Models;
using impacta_contatos_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<Contact>> GetContacts(int pageNumber = 0, int pageSize = 10)
        {
            return await _contactServices.GetAsync(pageNumber, pageSize);
        }

        [HttpPost]
        public async Task<Contact> PostContact(Contact contact)
        {
            await _contactServices.CreateAsync(contact);

            return contact;
        }
    }
}
