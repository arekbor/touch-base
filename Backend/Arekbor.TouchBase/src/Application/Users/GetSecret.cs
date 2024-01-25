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


public record FakeDataResult(string Messages);

public record GetFakeDataQuery(): IRequest<FakeDataResult[]>;

internal class GetFakeDataHandler : IRequestHandler<GetFakeDataQuery, FakeDataResult[]>
{
    public Task<FakeDataResult[]> Handle(GetFakeDataQuery request, CancellationToken cancellationToken)
    {
        var fakeData = new[]
        {
            new FakeDataResult("First Message"),
            new FakeDataResult("Second Message"),
            new FakeDataResult("Third Message"),
        };

        return Task.FromResult(fakeData);
    }
}


public record CustomDataResult(string CustomMessage);

public record GetCustomDataQuery : IRequest<CustomDataResult>;

internal class GetCustomDataHandler : IRequestHandler<GetCustomDataQuery, CustomDataResult>
{
    public Task<CustomDataResult> Handle(GetCustomDataQuery request, CancellationToken cancellationToken)
    {

        // Symulacja opóźnienia działania (np. emulacja czasu trwania operacji)
        Task.Delay(2000).Wait();

        // Zwróć wynik operacji
        return Task.FromResult(new CustomDataResult("hello world"));
    }
}