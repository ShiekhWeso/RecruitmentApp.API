FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RecruitmentApp.API/RecruitmentApp.API.csproj", "RecruitmentApp.API/"]
RUN dotnet restore "RecruitmentApp.API/RecruitmentApp.API.csproj"

COPY . .
WORKDIR "/src/RecruitmentApp.API"
RUN dotnet publish "RecruitmentApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RecruitmentApp.API.dll"]