#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["AXL.Api/AXL.Api.csproj", "AXL.Api/"]
#COPY ["AXL.Commons/AXL.Commons.csproj", "AXL.Commons/"]
#COPY ["AXL.Repositorys/AXL.Repositorys.csproj", "AXL.Repositorys/"]
#COPY ["AXL.Entitys/AXL.Entitys.csproj", "AXL.Entitys/"]
#COPY ["AXL.Dto/AXL.Dto.csproj", "AXL.Dto/"]
#COPY ["AXL.Repositorys.Contract/AXL.Repositorys.Contract.csproj", "AXL.Repositorys.Contract/"]
#COPY ["AXL.Services/AXL.Services.csproj", "AXL.Services/"]
#COPY ["AXL.Services.Contract/AXL.Services.Contract.csproj", "AXL.Services.Contract/"]
#RUN dotnet restore "AXL.Api/AXL.Api.csproj"
COPY . .
#WORKDIR "/src/AXL.Api"
#RUN dotnet build "AXL.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "AXL.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AXL.Api.dll"]