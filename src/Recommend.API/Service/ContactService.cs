using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        public async Task<List<Contact>> GetContactsByUserId(int userId)
        {
            _logger.LogTrace($"Enter info GetContactsByUserId with userId { userId}");
            try
            {
                var uri = $"{_contactServiceUrl}/api/contacts/{userId}";
                var response = await _httpClient.GetStringAsync(uri);

                if (!string.IsNullOrEmpty(response))
                {
                    var userInfo = JsonConvert.DeserializeObject<List<Contact>>(response);
                    _logger.LogTrace($"Completed GetContactsByUserId with userId {userId}");
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetContactsByUserId 在重试之后失败" + ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
