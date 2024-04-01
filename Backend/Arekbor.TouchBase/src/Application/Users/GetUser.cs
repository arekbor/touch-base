using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Users;

public record UserResult(string Email, string Username);

public record GetUserQuery() : IRequest<UserResult>;

internal class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserResult>
{
    private IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    public GetUserQueryHandler(
        IApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService) 
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<UserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var user = await _applicationDbContext
            .Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(id), cancellationToken)
                ?? throw new NotFoundException($"User ${Guid.Parse(id)} not found");
        
        return user.Adapt<UserResult>();
    }
}