FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
FROM base AS final
WORKDIR /app
COPY  ./  /app
ENTRYPOINT ["dotnet", "LCPC.Admin.dll"]
