using DnsClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using User.Identity.Dtos;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private readonly string _userServiceUrl;//="http://localhost:51929";

        public UserService(HttpClient httpClient, IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions, IDnsQuery dnsQuery)
        {
            _httpClient = httpClient;

            var address = dnsQuery.ResolveService("service.consul", serviceDiscoveryOptions.Value.UserServiceName);

            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;//这里返回的的localhost后为什么多个 "."


            var port = address.First().Port;

            _userServiceUrl = $"http://{host.Replace(".", "")}:{port}";

        }
        public async Task<int> CheckOrCreate(string phone)
        {
            var form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);
            var requestUrl = $"{_userServiceUrl}/api/users/check-or-create";
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userId = await response.Content.ReadAsStringAsync();

                int.TryParse(userId, out var intUserId);

                return intUserId;
            }

            return 0;
        }
    }
}
