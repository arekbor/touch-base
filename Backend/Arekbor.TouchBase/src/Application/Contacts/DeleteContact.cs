using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Arekbor.TouchBase.Application.Contacts;

public record DeleteContactCommand(Guid Id): IRequest<Unit>;

internal class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Unit>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    

    public DeleteContactCommandHandler(
        IApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService)
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.Id
            ?? throw new BadRequestException("User is not logged in");
 
        var contact = await _applicationDbContext.Contacts
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == Guid.Parse(userId), cancellationToken)
                ?? throw new NotFoundException($"Contact ${request.Id} not found");

        _applicationDbContext.Contacts.Remove(contact);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}