using MediatR;

namespace Arekbor.TouchBase.Application.Users;

public record SecretResult(string Message);

public record GetSecretQuery(): IRequest<SecretResult>;

internal class GetSecretHandler : IRequestHandler<GetSecretQuery, SecretResult>
{
    public Task<SecretResult> Handle(GetSecretQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SecretResult("Hello world"));
    }
}