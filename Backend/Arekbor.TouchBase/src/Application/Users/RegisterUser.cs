using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
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
            .Username();
         
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password();
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
        if (user is not null)
            throw new BadRequestException($"User with email: {request.Email} already exists");

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = _identityService.HashPassword(request.Password)
        };

        await _userRepository.AddAsync(newUser, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}