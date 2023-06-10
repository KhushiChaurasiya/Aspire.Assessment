using MediatR;
using Assignment.Contracts.DTO;
using Assignment.Contracts.Data;
using Assignment.Core.Exceptions;
using AutoMapper;

namespace Assignment.Providers.Handlers.Queries
{
    public class GetUserByUserNameQuery : IRequest<UserDTO>
    {
        public string Email { get; }
        public GetUserByUserNameQuery(string email)
        {
            Email = email;
        }
     
    }

    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, UserDTO>
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public GetUserByUserNameQueryHandler(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var app = await Task.FromResult(_repository.User.GetByEmail(request.Email));

            if (app == null)
            {
                throw new EntityNotFoundException($"No App found for Id {request.Email}");
            }

            return _mapper.Map<UserDTO>(app);
        }
    }
}