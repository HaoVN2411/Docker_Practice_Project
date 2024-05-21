#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NET1717_Lab01_ProductManagement.API/NET1717_Lab01_ProductManagement.API.csproj", "NET1717_Lab01_ProductManagement.API/"]
COPY ["NET1717_Lab01_ProductManagement.Repository/NET1717_Lab01_ProductManagement.Repository.csproj", "NET1717_Lab01_ProductManagement.Repository/"]
RUN dotnet restore "NET1717_Lab01_ProductManagement.API/NET1717_Lab01_ProductManagement.API.csproj"
COPY . .
WORKDIR "/src/NET1717_Lab01_ProductManagement.API"
RUN dotnet build "NET1717_Lab01_ProductManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NET1717_Lab01_ProductManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NET1717_Lab01_ProductManagement.API.dll"]