using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record UpdatePasswordCommand(string OldPassword, string NewPassword) : IRequest<Unit>;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand> 
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.NewPassword)
            .Password();
    }
}

internal class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePasswordCommandHandler(
        IUnitOfWork unitOfWork, 
        IIdentityService identityService,
        IUserRepository userRepository, 
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .GetAsync(_currentUserService.GetId(), cancellationToken)
                ??  throw new NotFoundException($"User ${_currentUserService.GetId()} not found");

        if (!_identityService.VerifyPasswordHash(user.Password!, request.OldPassword))
            throw new BadRequestException("Invalid old password.");

        user.Password = _identityService.HashPassword(request.NewPassword);

        _userRepository.Update(user);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}