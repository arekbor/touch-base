using Arekbor.TouchBase.Application.Common.Dtos;
using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record UpdateContactCommand(Guid Id, ContactBody ContactBody) : IRequest<Unit>;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactCommandValidator()
    {
        RuleFor(x => x.ContactBody)
            .SetValidator(new ContactBodyValidator());
    }
}

internal class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContactRepository _contactRepository;
    private readonly ICurrentUserService _currentUserService;
    public UpdateContactCommandHandler(
        IUnitOfWork unitOfWork,
        IContactRepository contactRepository,
        ICurrentUserService currentUserService) 
    {
        _unitOfWork = unitOfWork;
        _contactRepository = contactRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var contact = await _contactRepository
            .GetUserContactById(request.Id, Guid.Parse(userId), cancellationToken)
            ?? throw new NotFoundException($"Contact ${request.Id} not found");

        contact.Firstname = request.ContactBody.Firstname;
        contact.Surname = request.ContactBody.Surname;
        contact.Company = request.ContactBody.Company;
        contact.Phone = request.ContactBody.Phone;
        contact.Label = request.ContactBody.Label;
        contact.Email = request.ContactBody.Email;
        contact.Birthday = request.ContactBody.Birthday;
        contact.Relationship = request.ContactBody.Relationship;
        contact.Notes = request.ContactBody.Notes;

        _contactRepository.Update(contact);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}