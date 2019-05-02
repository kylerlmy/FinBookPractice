using DotNetCore.CAP;
using Recommend.API.Data;
using Recommend.API.IntegrationEvents;
using Recommend.API.Models;
using Recommend.API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEventHandlers
{
    public class ProjectCreatedIntegrationEventHandler : ICapSubscribe
    {
        private RecommendDbContext _context;
        private IUserService _userService;

        public ProjectCreatedIntegrationEventHandler(RecommendDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task CreateRecommendFromProject(ProjectCreatedIntegrationEvent integrationEvent)
        {
            var fromUser = await _userService.GetBaseUseInfoAsync(integrationEvent.UserId);

            var @event = new ProjectRecommend
            {
                ProjectId = integrationEvent.ProjectId,
                CreatedTime = integrationEvent.CreatedTime,
                UserId = integrationEvent.UserId,
                Company = integrationEvent.Company,
                FinState = integrationEvent.FinState,
                Introduction = integrationEvent.Introduction,
                ProjectAvatar = integrationEvent.ProjectAvatar,
                Tags = integrationEvent.Tags,
                RecommendTime = DateTime.Now,
                RecommendType = ERecommendType.Friend,
                FromUserId = fromUser.UserId,
                FromUserName = fromUser.Name,
                FromUserAvatar = fromUser.Avatar,
            };

           _context.

        }
    }
}
