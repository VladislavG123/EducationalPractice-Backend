FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
ENV ASPNETCORE_URLS=http://+:80  
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GreenPoint.Dotnet.WebApi/GreenPoint.Dotnet.WebApi.csproj", "GreenPoint.Dotnet.WebApi/"]
COPY ["GreenPoint.Dotnet.Services/GreenPoint.Dotnet.Services.csproj", "GreenPoint.Dotnet.Services/"]
COPY ["GreenPoint.Dotnet.Contracts/GreenPoint.Dotnet.Contracts.csproj", "GreenPoint.Dotnet.Contracts/"]
COPY ["GreenPoint.Dotnet.DataAccess/GreenPoint.Dotnet.DataAccess.csproj", "GreenPoint.Dotnet.DataAccess/"]
RUN dotnet restore "GreenPoint.Dotnet.WebApi/GreenPoint.Dotnet.WebApi.csproj"
COPY . .
WORKDIR "/src/GreenPoint.Dotnet.WebApi"
RUN dotnet build "GreenPoint.Dotnet.WebApi.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "GreenPoint.Dotnet.WebApi.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenPoint.Dotnet.WebApi.dll"]

