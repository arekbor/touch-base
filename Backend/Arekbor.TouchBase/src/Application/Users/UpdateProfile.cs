using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record UpdateProfileCommand(string Email, string Username) : IRequest<Unit>;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Username)
            .Username();
    }
}

internal class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public UpdateProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, 
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async  Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetAsync(_currentUserService.GetId(), cancellationToken)
                ??  throw new NotFoundException($"User ${_currentUserService.GetId()} not found");

        var userWithSameEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (userWithSameEmail?.Email is not null && userWithSameEmail.Email.Equals(user.Email))
            throw new BadRequestException($"User with email: {request.Email} already exists");

        user.Username = request.Username;
        user.Email = request.Email;

        _userRepository.Update(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}