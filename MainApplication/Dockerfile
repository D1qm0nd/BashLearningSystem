FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MainApplication/MainApplication.csproj", "MainApplication/"]
RUN dotnet restore "MainApplication/MainApplication.csproj"
COPY . .
WORKDIR "/src/MainApplication"
RUN dotnet build "MainApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainApplication.dll"]
