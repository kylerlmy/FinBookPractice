using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;
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
        private IHttpClient _httpClient;
        private readonly string _userServiceUrl;//="http://localhost:51929";
        private ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient, IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions, IDnsQuery dnsQuery, ILogger<UserService> logger)
        {
            _httpClient = httpClient;

            var address = dnsQuery.ResolveService("service.consul", serviceDiscoveryOptions.Value.UserServiceName);

            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;//这里返回的的localhost后为什么多个 "."


            var port = address.First().Port;

            _userServiceUrl = $"http://{host.Replace(".", "")}:{port}";

            _logger = logger;

        }
        public async Task<UserInfo> CheckOrCreate(string phone)
        {
            var form = new Dictionary<string, string> { { "phone", phone } };
            var requestUrl = $"{_userServiceUrl}/api/users/check-or-create";

            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsync(requestUrl, form);
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckOrCreate 在重试之后失败" + ex.Message + ex.StackTrace);
            }

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                //var userId = await response.Content.ReadAsStringAsync();
                //int.TryParse(userId, out var intUserId);
                //return intUserId;
                var result = await response.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(result);

                return userInfo;
            }

            return null;
        }
    }
}
