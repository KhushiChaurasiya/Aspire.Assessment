using MediatR;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data;
using Assignment.Core.Exceptions;
using AutoMapper;

namespace Assignment.Providers.Handlers.Queries
{
    public class GetAppByIdQuery : IRequest<AppDTO>
    {
        public int AppId { get; }
        public GetAppByIdQuery(int appId)
        {
            AppId = appId;
        }
    }

    public class GetAppByIdQueryHandler : IRequestHandler<GetAppByIdQuery, AppDTO>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetAppByIdQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AppDTO> Handle(GetAppByIdQuery request, CancellationToken cancellationToken)
        {
            var app = await Task.FromResult(_repository.App.Get(request.AppId));

            if (app == null)
            {
                throw new EntityNotFoundException($"No App found for Id {request.AppId}");
            }

            return _mapper.Map<AppDTO>(app);
        }
    }
}