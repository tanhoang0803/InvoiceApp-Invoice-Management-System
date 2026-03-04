FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY invoice_database/invoice_database.csproj invoice_database/
RUN dotnet restore invoice_database/invoice_database.csproj

COPY . .
RUN dotnet publish invoice_database/invoice_database.csproj \
    --configuration Release \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "invoice_database.dll"]
