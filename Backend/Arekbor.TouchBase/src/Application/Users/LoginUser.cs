using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using FluentValidation;
using MediatR;

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
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IIdentityService identityService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
    }
    
    public async Task<TokensResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken) 
            ?? throw new BadRequestException("Invalid email or password.");

        if (!_identityService.VerifyPasswordHash(user.Password!, request.Password))
            throw new BadRequestException("Invalid email or password.");

        var (accessToken, refreshToken) = await _identityService
            .Authorize(user.Id, cancellationToken);

        return new TokensResult(accessToken, refreshToken);
    }
}