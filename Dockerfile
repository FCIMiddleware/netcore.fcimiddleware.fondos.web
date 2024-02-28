#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/netcore.fcimiddleware.fondos.web/netcore.fcimiddleware.fondos.web.csproj", "src/netcore.fcimiddleware.fondos.web/"]
RUN dotnet restore "src/netcore.fcimiddleware.fondos.web/netcore.fcimiddleware.fondos.web.csproj"
COPY . .
WORKDIR "/src/src/netcore.fcimiddleware.fondos.web"
RUN dotnet build "netcore.fcimiddleware.fondos.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "netcore.fcimiddleware.fondos.web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "netcore.fcimiddleware.fondos.web.dll"]