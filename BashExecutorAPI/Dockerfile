FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BashExecutoAPI/BashExecutoAPI.csproj", "BashExecutoAPI/"]
RUN dotnet restore "BashExecutoAPI/BashExecutoAPI.csproj"
COPY . .
WORKDIR "/src/BashExecutoAPI"
RUN dotnet build "BashExecutoAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BashExecutoAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BashExecutoAPI.dll"]
