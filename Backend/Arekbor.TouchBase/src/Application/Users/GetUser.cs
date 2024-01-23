using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record UserResult(string Email, string Username);

public record GetUserQuery() : IRequest<UserResult>;

internal class GetUserHandler : IRequestHandler<GetUserQuery, UserResult>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    public GetUserHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository) 
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<UserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id;
        if (id is null)
        {
            throw new NotFoundException($"User with ID: {id} not found");
        }

        var user = await _userRepository
            .GetAsync(Guid.Parse(id), cancellationToken);
        
        return user.Adapt<UserResult>();
    }
}