FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY NScrapyWebConsole/NScrapyWebConsole.csproj NScrapyWebConsole/
RUN dotnet restore NScrapyWebConsole/NScrapyWebConsole.csproj
COPY . .
WORKDIR /src/NScrapyWebConsole
RUN dotnet build NScrapyWebConsole.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NScrapyWebConsole.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NScrapyWebConsole.dll"]
