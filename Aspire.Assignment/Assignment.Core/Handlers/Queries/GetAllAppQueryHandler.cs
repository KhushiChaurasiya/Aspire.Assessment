using AutoMapper;
using Assignment.Contracts.Data;
using Assignment.Contracts.Data.Entities;
using Assignment.Contracts.DTO;
using MediatR;
using System.Linq;
using Assignment.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Assignment.Providers.Handlers.Queries
{
    public class GetAllAppQuery : IRequest<IEnumerable<AppDTO>>
    {
    }

    public class GetAllAppQueryHandler : IRequestHandler<GetAllAppQuery, IEnumerable<AppDTO>>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllAppQueryHandler> _logger;

        public GetAllAppQueryHandler(IUnitOfWork repository, IMapper mapper, ILogger<GetAllAppQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AppDTO>> Handle(GetAllAppQuery request, CancellationToken cancellationToken)
        {
            var entities = await Task.FromResult(_repository.App.GetAll());
            if(entities == null)
            {
               throw new EntityNotFoundException($"No data found!");
            }
            return _mapper.Map<IEnumerable<AppDTO>>(entities);
        }
    }
}