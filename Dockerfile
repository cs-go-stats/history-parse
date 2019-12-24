FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app
COPY /out/pub ./
ENTRYPOINT ["dotnet", "CSGOStats.Services.HistoryParse.dll"]