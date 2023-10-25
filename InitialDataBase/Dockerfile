FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["InitialDataBase/InitialDataBase.csproj", "InitialDataBase/"]
RUN dotnet restore "InitialDataBase/InitialDataBase.csproj"
COPY . .
WORKDIR "/src/InitialDataBase"
RUN dotnet build "InitialDataBase.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InitialDataBase.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InitialDataBase.dll"]
