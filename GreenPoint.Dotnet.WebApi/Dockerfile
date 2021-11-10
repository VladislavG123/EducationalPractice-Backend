FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GreenPoint.Dotnet.WebApi/GreenPoint.Dotnet.WebApi.csproj", "GreenPoint.Dotnet.WebApi/"]
COPY ["GreenPoint.Dotnet.Services/GreenPoint.Dotnet.Services.csproj", "GreenPoint.Dotnet.Services/"]
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