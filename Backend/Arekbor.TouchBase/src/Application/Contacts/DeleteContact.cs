using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public record DeleteContactCommand(Guid Id): IRequest<Unit>;

internal class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IContactRepository _contactRepository;

    public DeleteContactCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IContactRepository contactRepository)
    {
        _unitOfWork =  unitOfWork;
        _currentUserService = currentUserService;
        _contactRepository = contactRepository;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _contactRepository
            .GetUserContactById(request.Id, _currentUserService.GetId(), cancellationToken)
                ?? throw new NotFoundException($"Contact ${request.Id} not found");

        _contactRepository.Delete(contact);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}