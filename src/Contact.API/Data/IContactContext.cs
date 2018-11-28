using Contact.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public interface IContactContext
    {
        IMongoCollection<ContactBook> ContactBooks { get; }
        IMongoCollection<ContactApplyRequest> ContactApplyRequests { get; }
    }
}
