using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Contact.API.Dtos;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;

namespace Contact.API.Service
{
    public class UserService:IUserService
    {
        private string _userServiceUrl;
        private ILogger<UserService> _logger;
        private IHttpClient _httpClient;

        public UserService(IHttpClient httpClient,
            IOptions<ServiceDiscoveryOptions> serviceDiscoveryOptions,
            IDnsQuery dnsQuery,
            ILogger<UserService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;

            var address = dnsQuery.ResolveService("service.consul", serviceDiscoveryOptions.Value.UserServiceName);

            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;//这里返回的的localhost后为什么多个 "."


            var port = address.First().Port;

            _userServiceUrl = $"http://{host.Replace(".", "")}:{port}";
        }
        public async Task<UserIdentity> GetBaseUseInfoAsync(int userId)
        {
            _logger.LogTrace($"Enter info GetBaseUseInfoAsync with userId { userId}");
            try
            {
                var uri = $"{_userServiceUrl}/api/users/baseinfo/{userId}";
               var  response = await _httpClient.GetStringAsync(uri);

                if (string.IsNullOrEmpty(response))
                {
                    //var userId = await response.Content.ReadAsStringAsync();
                    //int.TryParse(userId, out var intUserId);
                    //return intUserId;
                    var userInfo = JsonConvert.DeserializeObject<UserIdentity>(response);
                    _logger.LogTrace($"Completed GetBaseUseInfoAsync with userId { userInfo.UserId}");
                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetBaseUseInfoAsync 在重试之后失败" + ex.Message + ex.StackTrace);
            }

           

            return null;
        }
    }
}
