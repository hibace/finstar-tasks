FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaskManagementService.API/TaskManagementService.API.csproj", "TaskManagementService.API/"]
COPY ["TaskManagementService.Core/TaskManagementService.Core.csproj", "TaskManagementService.Core/"]
COPY ["TaskManagementService.Infrastructure/TaskManagementService.Infrastructure.csproj", "TaskManagementService.Infrastructure/"]
COPY ["TaskManagementService.Application/TaskManagementService.Application.csproj", "TaskManagementService.Application/"]
COPY ["TaskManagementService.Domain/TaskManagementService.Domain.csproj", "TaskManagementService.Domain/"]
COPY ["TaskManagementService.Shared/TaskManagementService.Shared.csproj", "TaskManagementService.Shared/"]
RUN dotnet restore "TaskManagementService.API/TaskManagementService.API.csproj"
COPY . .
WORKDIR "/src/TaskManagementService.API"
RUN dotnet build "TaskManagementService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManagementService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementService.API.dll"] 