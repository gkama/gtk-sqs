FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 8003

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["sqs.core/sqs.core.csproj", "sqs.core/"]
COPY ["sqs.data/sqs.data.csproj", "sqs.data/"]
COPY ["sqs.services/sqs.services.csproj", "sqs.services/"]
RUN dotnet restore "sqs.core/sqs.core.csproj"
COPY . .
WORKDIR "/src/sqs.core"
RUN dotnet build "sqs.core.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "sqs.core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sqs.core.dll"]