FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GreenPoint.Dotnet.WebAdmin/GreenPoint.Dotnet.WebAdmin.csproj", "GreenPoint.Dotnet.WebAdmin/"]
COPY ["GreenPoint.Dotnet.DataAccess/GreenPoint.Dotnet.DataAccess.csproj", "GreenPoint.Dotnet.DataAccess/"]
RUN dotnet restore "GreenPoint.Dotnet.WebAdmin/GreenPoint.Dotnet.WebAdmin.csproj"
COPY . .
WORKDIR "/src/GreenPoint.Dotnet.WebAdmin"
RUN dotnet build "GreenPoint.Dotnet.WebAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenPoint.Dotnet.WebAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenPoint.Dotnet.WebAdmin.dll"]
