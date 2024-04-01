using Arekbor.TouchBase.Application.Common.Exceptions;
using Arekbor.TouchBase.Application.Common.Interfaces;
using Arekbor.TouchBase.Application.Common.Models;
using MediatR;

namespace Arekbor.TouchBase.Application.Contacts;

public class GetContactsResult
{
    public Guid Id { get; set; }
    public string? Firstname { get; set; }
    public string? Surname { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public record GetContactsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetContactsResult>>;

internal class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, PaginatedList<GetContactsResult>>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    public GetContactsQueryHandler(
        IApplicationDbContext applicationDbContext, 
        ICurrentUserService currentUserService)
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<GetContactsResult>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
    {
        var id = _currentUserService.Id 
            ?? throw new BadRequestException("User is not logged in");

        var query = _applicationDbContext
            .Contacts
            .Where(c => c.UserId == Guid.Parse(id))
            .Select(c => new GetContactsResult
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Surname = c.Surname,
                Phone = c.Phone,
                Email = c.Email
            });

        return await PaginatedList<GetContactsResult>
            .CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}