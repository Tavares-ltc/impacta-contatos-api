using impacta_contatos_api.Models;
using Bogus;

namespace impacta_contatos_api.Helpers
{
    public class ContactMockGenerator
    {
        private readonly List<string> legalFields = new List<string>
        {
            "Advogado",
            "Juiz",
            "Promotor",
            "Defensor Público",
            "Procurador",
            "Delegado",
        };

        public List<Contact> GenerateLegalContacts(int numberOfContacts)
        {
            var faker = new Faker<Contact>()
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.LegalField, f => f.PickRandom(legalFields))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Image, f => f.Internet.Avatar())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Description, f => f.Lorem.Paragraph());

            return faker.Generate(numberOfContacts);
        }
    }
}
