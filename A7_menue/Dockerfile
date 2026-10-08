# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["A7_menue.csproj", "./"]
RUN dotnet restore "A7_menue.csproj"
COPY . .
RUN dotnet publish "A7_menue.csproj" -c Release -o /app/publish --no-restore

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "A7_menue.dll"]