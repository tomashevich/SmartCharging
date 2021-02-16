using MediatR;

using System;

namespace SmartCharge.Application.Queries
{   
    public class GetChargeGroupQuery : IRequest<GetChargeGroupDto>
    {
        public Guid Id { get; set; }
    }
}
