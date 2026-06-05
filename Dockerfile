FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app

RUN addgroup -S appgroup && adduser -S appuser -G appgroup

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet publish "LavenderFlow-API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY entrypoint.sh .

RUN chmod +x entrypoint.sh \
    && chown -R appuser:appgroup /app

ENV ASPNETCORE_URLS="http://+:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV DOTNET_RUNNING_IN_CONTAINER="true"

USER appuser

EXPOSE 5000

ENTRYPOINT ["./entrypoint.sh"]