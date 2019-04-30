using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Applications.Commands;
using Project.Domain.AggregatesModel;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProjectController : BaseController
    {
        private IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
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

        public async Task<IActionResult> JoinProject([FromBody] ProjectContributor contributor)
        {
            var command = new JoinProjectCommand { Contributor = contributor };
            await _mediator.Send(command);

            return Ok();
        }

    }
}
