using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Recommend.API.Dtos;
using Resilience.Http;

namespace Recommend.API.Service
{
    public class ContactService : IContactService
    {
        private IHttpClient _httpClient;
        private string _contactServiceUrl;
        private ILogger<ContactService> _logger;

        public ContactService(IHttpClient httpClient,
            IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions,
            IDnsQuery dnsQuery,
            ILogger<ContactService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;

            var address = dnsQuery.ResolveService("service.consul", serviceDiscoveryOptions.Value.ContactServiceName);

            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;//这里返回的的localhost后为什么多个 "."


            var port = address.First().Port;

            _contactServiceUrl = $"http://{host.Replace(".", "")}:{port}";


        }
        public Task<List<Contact>> GetContactsByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
