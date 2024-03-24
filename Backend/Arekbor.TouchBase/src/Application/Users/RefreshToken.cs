using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Users.Dtos;
using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record RefreshTokenQuery(string RefreshToken) : IRequest<TokensResult>;

internal class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, TokensResult>
{
    private readonly IIdentityService _identityService;
    public RefreshTokenQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<TokensResult> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken) = await _identityService
            .Refresh(request.RefreshToken, cancellationToken);

        return new TokensResult(accessToken, refreshToken);
    }
}