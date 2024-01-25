FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build

WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS runtime
EXPOSE 80
WORKDIR /app

COPY --from=build /app/out .
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT [ "dotnet", "Api.dll" ]