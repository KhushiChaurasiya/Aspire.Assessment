using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{

    public class GetAllRoleQuery : IRequest<IEnumerable<RoleDTO>>
    {
    }

    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, IEnumerable<RoleDTO>>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDTO>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var entities = await Task.FromResult(_repository.Role.GetAll());
            return _mapper.Map<IEnumerable<RoleDTO>>(entities);
        }

    }
}
