﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Models;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactApplyRequestRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        public async Task<bool> AddRequestAsync(ContactApplyRequest request, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == request.UserId && r.ApplierId == request.ApplierId);

#if !overTimeVersion

            if ((await _contactContext.ContactApplyRequests.CountAsync(filter)) > 0)
            {
                var update = Builders<ContactApplyRequest>.Update.Set(r => r.ApplyTime, DateTime.Now);
                var result = await _contactContext.ContactApplyRequests.UpdateOneAsync(filter, update, null, cancellationToken);

                return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
            }
#else
            var update = Builders<ContactApplyRequest>.Update.Set(r => r.ApplyTime, DateTime.Now);
            var options = new UpdateOptions { IsUpsert = true };
            _contactContext.ContactApplyRequests.UpdateOne(filter, update, options, cancellationToken);
#endif

            await _contactContext.ContactApplyRequests.InsertOneAsync(request, null, cancellationToken);

            return true;
        }

        public async Task<bool> ApprovalAsync(int userId, int applierId, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == userId && r.ApplierId == applierId);


            var update = Builders<ContactApplyRequest>.Update
                .Set(r => r.Approvaled,1)
                .Set(r=>r.HandelTime,DateTime.Now);
            var result = await _contactContext.ContactApplyRequests.UpdateOneAsync(filter, update, null, cancellationToken);

            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }

        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _contactContext.ContactApplyRequests.FindAsync(c => c.UserId == userId)).ToList(cancellationToken);
        }
    }
}
