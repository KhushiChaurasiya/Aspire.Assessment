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
    public class GetAllUserReportQuery : IRequest<int>
    {
    }
    public class GetAllUserReportQueryHandler : IRequestHandler<GetAllUserReportQuery, int>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetAllUserReportQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(GetAllUserReportQuery request, CancellationToken cancellationToken)
        {

           var data =  _repository.User.Count();
            await _repository.CommitAsync();

            return data;
        }

    }
}
