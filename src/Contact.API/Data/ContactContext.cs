using Contact.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public class ContactContext : IContactContext
    {
        private IMongoDatabase _database;
        private IMongoCollection<ContactBook> _collection;
        private Settings _appSettings;

        public ContactContext(IOptionsSnapshot<Settings> settings)
        {
            _appSettings = settings.Value;

            var client = new MongoClient(_appSettings.ConnectionString);
            _database = client.GetDatabase(_appSettings.Database);
        }
        private void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _database.ListCollections().ToList();
            var collectionNames = new List<string>();

            collectionList.ForEach(b => collectionNames.Add(b["name"].AsString));

            if (collectionNames.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        //Mongo中，每个Database会有多个Collections，每个Collections相当于关系数据库中的一个表，数据都存储到Collections中

        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                CheckAndCreateCollection("ContactBook");
                return _database.GetCollection<ContactBook>("ContactBook");
            }
        }


        /// <summary>
        /// 好友的申请请求
        /// </summary>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequests 
        {
            get
            {
                CheckAndCreateCollection("ContactApplyRequest");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequest");
            }
        }

    }
}
