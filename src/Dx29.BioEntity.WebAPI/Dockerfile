#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src
COPY ["Dx29.BioEntity.WebAPI/Dx29.BioEntity.WebAPI.csproj", "Dx29.BioEntity.WebAPI/"]
RUN dotnet restore "Dx29.BioEntity.WebAPI/Dx29.BioEntity.WebAPI.csproj"
COPY . .
WORKDIR "/src/Dx29.BioEntity.WebAPI"
RUN dotnet build "Dx29.BioEntity.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dx29.BioEntity.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dx29.BioEntity.WebAPI.dll"]
