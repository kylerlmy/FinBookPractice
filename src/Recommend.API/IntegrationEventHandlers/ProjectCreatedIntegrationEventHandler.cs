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
        private IContactService _contactService;

        public ProjectCreatedIntegrationEventHandler(RecommendDbContext context, IUserService userService, IContactService contactService)
        {
            _context = context;
            _userService = userService;
            _contactService = contactService;
        }

        [CapSubscribe("finbook.projectapi.projectcreated")]
        public async Task CreateRecommendFromProject(ProjectCreatedIntegrationEvent @event)
        {
            var fromUser = await _userService.GetBaseUseInfoAsync(@event.UserId);
            var contacts = await _contactService.GetContactsByUserId(@event.UserId);

            foreach (var contact in contacts)
            {

                var recommend = new ProjectRecommend
                {
                    ProjectId = @event.ProjectId,
                    CreatedTime = @event.CreatedTime,
                    Company = @event.Company,
                    FinState = @event.FinState,
                    Introduction = @event.Introduction,
                    ProjectAvatar = @event.ProjectAvatar,
                    Tags = @event.Tags,
                    RecommendTime = DateTime.Now,
                    RecommendType = ERecommendType.Friend,
                    FromUserId = fromUser.UserId,
                    FromUserName = fromUser.Name,
                    FromUserAvatar = fromUser.Avatar,

                    UserId = contact.UserId,

                };

                _context.Recommends.Add(recommend);
            }



            await _context.SaveChangesAsync();

        }
    }
}
