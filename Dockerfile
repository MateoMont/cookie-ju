# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebGalletas/WebGalletas.csproj", "WebGalletas/"]
RUN dotnet restore "WebGalletas/WebGalletas.csproj"
COPY . .
WORKDIR "/src/WebGalletas"
RUN dotnet build "WebGalletas.csproj" -c Release -o /app/build

# Etapa de publish
FROM build AS publish
RUN dotnet publish "WebGalletas.csproj" -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebGalletas.dll"]
