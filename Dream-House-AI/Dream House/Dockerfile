FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Dream House/Dream House.csproj", "Dream House/"]
RUN dotnet restore "Dream House/Dream House.csproj"

COPY . .
WORKDIR "/src/Dream House"
RUN dotnet build "Dream House.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dream House.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dream House.dll"]
