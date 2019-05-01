using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.API.Applications.Queries;
using Project.API.Applications.Service;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProjectController : BaseController
    {
        private IMediator _mediator;
        private IRecommendService _recommendService;
        private IProjectQueries _projectQueries;

        public ProjectController(IMediator mediator,
            IRecommendService recommendService,
            IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpGet]
        [Route("")]

        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectQueries.GetProjectByUserId(UserIdentity.UserId);
            return Ok(projects);
        }

        [HttpGet]
        [Route("my/{projectId}")]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var project = await _projectQueries.GetProjectDetail(projectId);
            if (project.UserId == UserIdentity.UserId)
            {
                return Ok(project);
            }
            else
            {
                return BadRequest("无权限查看该项目");
            }
        }

        /// <summary>
        /// api/project/remommends/1
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("remommends/{projectId}")]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {
            if (await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId))//确定项目是否在推荐列表之中
            {
                var project = await _projectQueries.GetProjectDetail(projectId);
                return Ok(project);
            }
            else
            {
                return BadRequest("无权限查看该项目");
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateProject([FromBody]Domain.AggregatesModel.Project project)
        {
            var command = new CreateProjectCommand { Project = project };
            var projectResponse = await _mediator.Send(command);

            return Ok(projectResponse);

        }
        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            if (await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看项目的权限");
            }

            var command = new ViewProjectCommand
            {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId
            };
            await _mediator.Send(command);

            return Ok();

        }

        [HttpPut]
        [Route("join/{projectId}")]

        public async Task<IActionResult> JoinProject(int projectId,[FromBody] ProjectContributor contributor)
        {
            if (await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看项目的权限");
            }

            var command = new JoinProjectCommand { Contributor = contributor };
            await _mediator.Send(command);

            return Ok();
        }

    }
}
