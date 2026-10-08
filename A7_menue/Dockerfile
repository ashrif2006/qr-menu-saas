FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV PORT=7860
EXPOSE 7860

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["A7_menue.csproj", "./"]
RUN dotnet restore "A7_menue.csproj"
COPY . .
RUN dotnet publish "A7_menue.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "A7_menue.dll"]