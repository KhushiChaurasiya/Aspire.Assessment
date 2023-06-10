using Assignment.Contracts.Data;
using Assignment.Contracts.DTO;
using Assignment.Core.Exceptions;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Core.Handlers.Queries
{
    public class DeleteAppByIdQuery : IRequest<AppDTO>
    {
        public int AppId { get; }
        public DeleteAppByIdQuery(int appId)
        {
            AppId = appId;
        }
    }

    public class DeleteAppByIdQueryHandler : IRequestHandler<DeleteAppByIdQuery, AppDTO>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public DeleteAppByIdQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AppDTO> Handle(DeleteAppByIdQuery request, CancellationToken cancellationToken)
        {

           _repository.App.Delete(request.AppId);
            await _repository.CommitAsync();

            return null;
        }
    }
}
