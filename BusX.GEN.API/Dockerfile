FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BusX.GEN.API/BusX.GEN.API.csproj", "BusX.GEN.API/"]
COPY ["BusX.Extensions/BusX.Extensions.csproj", "BusX.Extensions/"]
COPY ["BusX.GENAppService/BusX.GENAppService.csproj", "BusX.GENAppService/"]
COPY ["BusXAppServiceModels/BusXAppServiceModels.csproj", "BusXAppServiceModels/"]

RUN dotnet restore "BusX.GEN.API/BusX.GEN.API.csproj"

COPY . .

WORKDIR "/src/BusX.GEN.API"
RUN dotnet build "BusX.GEN.API.csproj" -c Release -o /app/build
RUN dotnet publish "BusX.GEN.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Sqlite gomulu
COPY BusX.Data/Sqlite/busx.sqlite ./busx.sqlite

ENTRYPOINT ["dotnet", "BusX.GEN.API.dll"]
