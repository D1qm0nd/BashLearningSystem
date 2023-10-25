FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LearningSite.WebApp/LearningSite.WebApp.csproj", "LearningSite.WebApp/"]
RUN dotnet restore "LearningSite.WebApp/LearningSite.WebApp.csproj"
COPY . .
WORKDIR "/src/LearningSite.WebApp"
RUN dotnet build "LearningSite.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningSite.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningSite.WebApp.dll"]
