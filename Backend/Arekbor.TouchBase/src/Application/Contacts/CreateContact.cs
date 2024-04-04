using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using Arekbor.TouchBase.Domain.Entities;
using FluentValidation;
using Mapster;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record CreateContactCommand(): ContactBody, IRequest<Unit>;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        Include(new ContactBodyValidator());
    }
}

internal class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public CreateContactCommandHandler(
        IUnitOfWork unitOfWork,
        IContactRepository contactRepository,
        ICurrentUserService currentUserService) 
    {
        _unitOfWork = unitOfWork;
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = request.Adapt<Contact>();
        
        contact.UserId = _currentUserService.GetId();

        await _contactRepository.AddAsync(contact, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}