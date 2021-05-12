#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Succubus.Web/Succubus.Web.csproj", "Succubus.Web/"]
COPY ["Succubus.Application/Succubus.Application.csproj", "Succubus.Application/"]
COPY ["Succubus.Core/Succubus.Core.csproj", "Succubus.Core/"]
COPY ["Succubus.Infrastructure/Succubus.Infrastructure.csproj", "Succubus.Infrastructure/"]
RUN dotnet restore "Succubus.Web/Succubus.Web.csproj"
COPY . .
WORKDIR "/src/Succubus.Web"
RUN dotnet build "Succubus.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Succubus.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "Succubus.Web.dll"]