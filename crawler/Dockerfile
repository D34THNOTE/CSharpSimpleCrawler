FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["crawler/crawler.csproj", "crawler/"]
RUN dotnet restore "crawler/crawler.csproj"
COPY . .
WORKDIR "/src/crawler"
RUN dotnet build "crawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "crawler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "crawler.dll"]
