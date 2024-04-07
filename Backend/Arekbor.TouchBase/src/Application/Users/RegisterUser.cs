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
            .WithMessage("{PropertyName} cannot contain more than 40 characters.");
         
        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(40)
            .WithMessage("{PropertyName} cannot contain more than 40 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8)
            .WithMessage("{PropertyName} must be at least 8 characters long.")
            .MaximumLength(40)
            .WithMessage("{PropertyName} cannot contain more than 40 characters.")
            .Matches(@"[A-Z]+")
            .WithMessage("{PropertyName} must contain at least one uppercase letter.")
            .Matches(@"[a-z]+")
            .WithMessage("{PropertyName} must contain at least one lowercase letter.")
            .Matches(@"[0-9]+")
            .WithMessage("{PropertyName} must contain at least one number.")
            .Matches(@"[][""!@#$%^&*(){}:;<>,.?/+_=|'~\\-]")
            .WithMessage("{PropertyName} must contain at least one special character.");
    }
}

internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IIdentityService identityService) 
    {
        _unitOfWork = unitOfWork;
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
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}