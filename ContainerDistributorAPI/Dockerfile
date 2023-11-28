FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

CMD [&quot;systemd&quot;]
# Обновление пакетов и установка зависимостей
RUN apt-get update && \
    apt-get install -y apt-transport-https ca-certificates curl gnupg-agent software-properties-common

RUN apt install systemd

# Добавление официального ключа Docker GPG
RUN curl -fsSL https://download.docker.com/linux/debian/gpg | apt-key add -

# Добавление официального репозитория Docker
RUN add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/debian $(lsb_release -cs) stable"

# Установка Docker Engine
RUN apt-get update && \
    apt-get install -y docker-ce docker-ce-cli containerd.io 


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ContainerDistributor/ContainerDistributor.csproj", "ContainerDistributor/"]
RUN dotnet restore "ContainerDistributor/ContainerDistributor.csproj"
COPY . .
WORKDIR "/src/ContainerDistributor"
RUN dotnet build "ContainerDistributor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContainerDistributor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContainerDistributor.dll"]

