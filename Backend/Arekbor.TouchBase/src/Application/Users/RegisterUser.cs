using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record RegisterUserCommand(string Email, string Username, string Password) : IRequest<Unit>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {  
        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.");
         
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8)
            .WithMessage("{PropertyName} length must be at least 8.")
            .MaximumLength(40)
            .WithMessage("{PropertyName} length must be at most 40.")
            .Matches(@"[A-Z]+")
            .WithMessage("{PropertyName} must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("{PropertyName} must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("{PropertyName} must contain at least one number.")
            .Matches(@"[][""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]")
            .WithMessage("{PropertyName} must contain one or more special characters.");
    }
}

internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IIdentityService identityService) 
    {
        _userRepository = userRepository;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user != null)
        {
            throw new BadRequestException($"User with email: {request.Email} already exists");
        }

        var hashedPassword = _identityService.HashPassword(request.Password);

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = hashedPassword
        };

        await _userRepository.AddAsync(newUser, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}