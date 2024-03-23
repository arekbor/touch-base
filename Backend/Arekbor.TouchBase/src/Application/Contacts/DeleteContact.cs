using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record DeleteContactCommand(Guid Id): IRequest<Unit>;

internal class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IContactRepository _contactRepository;

    public DeleteContactCommandHandler(
        ICurrentUserService currentUserService,
        IContactRepository contactRepository)
    {
        _currentUserService = currentUserService;
        _contactRepository = contactRepository;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id
            ?? throw new BadRequestException("User is not logged in");
 
        var contact = await _contactRepository
            .GetUserContactById(request.Id, Guid.Parse(id), cancellationToken)
            ?? throw new NotFoundException($"Contact ${request.Id} not found");

        _contactRepository.Delete(contact);
        await _contactRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}