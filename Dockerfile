# Build the application.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SpendSmart/SpendSmart.csproj SpendSmart/
RUN dotnet restore SpendSmart/SpendSmart.csproj

COPY SpendSmart/ SpendSmart/
RUN dotnet publish SpendSmart/SpendSmart.csproj \
    --configuration Release \
    --output /out \
    --no-restore \
    -p:UseAppHost=false

# Package the application for running.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

RUN mkdir -p /app/data && chown app:app /app/data

COPY --from=build /out/ ./

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

USER app
ENTRYPOINT ["dotnet", "SpendSmart.dll"]
