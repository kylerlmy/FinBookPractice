using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class CreateOrderCommand : IRequest<Domain.AggregatesModel.Project>
    {
        public Domain.AggregatesModel.Project Project { get; st; }
    }
}
