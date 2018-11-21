using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {

        private readonly ContactContext _contactContext;
        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        public async Task<bool> UpdateContactInfo(BaseUseInfo userinfo, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userinfo.UserId, null, cancellationToken)).FirstOrDefault();

            if (contactBook == null)
            {
                //throw new Exception($"Wrong user Id for update contact info UserId{userinfo.UserId}");
                return true;
            }

            var contactIds = contactBook.Contacts.Select(c => c.UserId);

            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == userinfo.UserId)
                );

            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", userinfo.Name)
               .Set("Contacts.$.Avatar", userinfo.Avatar)
               .Set("Contacts.$.Company", userinfo.Company)
               .Set("Contacts.$.Title", userinfo.Title);

            var updateResult = _contactContext.ContactBooks.UpdateMany(filter, update);

            return updateResult.MatchedCount == updateResult.ModifiedCount;

        }
    }
}
