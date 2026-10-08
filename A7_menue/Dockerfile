FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["A7_menue.csproj", "./"]
RUN dotnet restore "A7_menue.csproj"
COPY . .
RUN dotnet build "A7_menue.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "A7_menue.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "A7_menue.dll"]