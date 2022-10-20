#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AXL/AXL.csproj", "AXL/"]
RUN dotnet restore "AXL/AXL.csproj"
COPY . .
WORKDIR "/src/AXL"
RUN dotnet build "AXL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AXL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AXL.dll"]