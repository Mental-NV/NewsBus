#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/NewsBus.DownloaderService/NewsBus.DownloaderService.csproj", "src/NewsBus.DownloaderService/"]
COPY ["src/NewsBus.Application/NewsBus.Application.csproj", "src/NewsBus.Application/"]
COPY ["src/NewsBus.Domain/NewsBus.Domain.csproj", "src/NewsBus.Domain/"]
COPY ["src/NewsBus.Infrastructure/NewsBus.Infrastructure.csproj", "src/NewsBus.Infrastructure/"]
RUN dotnet restore "src/NewsBus.DownloaderService/NewsBus.DownloaderService.csproj"
COPY . .
WORKDIR "/src/src/NewsBus.DownloaderService"
RUN dotnet build "NewsBus.DownloaderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NewsBus.DownloaderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NewsBus.DownloaderService.dll"]