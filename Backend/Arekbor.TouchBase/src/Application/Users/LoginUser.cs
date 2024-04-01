using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Users;

public record LoginUserQuery(string Email, string Password) : IRequest<TokensResult>;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {   
        RuleFor(x => x.Email)
            .EmailAddress();
            
        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull();
    }
}

internal class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, TokensResult>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IIdentityService _identityService;
    public LoginUserQueryHandler(
        IApplicationDbContext applicationDbContext,
        IIdentityService identityService)
    {
        _applicationDbContext = applicationDbContext;
        _identityService = identityService;
    }
    
    public async Task<TokensResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken)
                ?? throw new BadRequestException("Email or password is invalid");

        if (!_identityService.VerifyPasswordHash(user.Password!, request.Password))
            throw new BadRequestException("Email or password is invalid");

        var (accessToken, refreshToken) = await _identityService
            .Authorize(user.Id, cancellationToken);

        return new TokensResult(accessToken, refreshToken);
    }
}