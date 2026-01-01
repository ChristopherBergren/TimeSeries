# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.sln .
COPY TimeSeries/TimeSeries.csproj ./TimeSeries/
RUN dotnet restore

# Copy all files
COPY . .

# Build the project
RUN dotnet publish TimeSeries/TimeSeries.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Expose ports used by the API
ENV DOTNET_URLS=http://+:5087
EXPOSE 5087

# Environment variables for Kestrel
ENV DOTNET_URLS=http://+:5087

# Run the app in Debug mode 
ENTRYPOINT ["dotnet", "TimeSeries.dll"]