FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApp.LearningSystem/WebApp.LearningSystem.csproj", "WebApp.LearningSystem/"]
RUN dotnet restore "WebApp.LearningSystem/WebApp.LearningSystem.csproj"
COPY . .
WORKDIR "/src/WebApp.LearningSystem"
RUN dotnet build "WebApp.LearningSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.LearningSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.LearningSystem.dll"]
