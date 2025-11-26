FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["DLyah Boutique System.sln", "."]
COPY ["DLyah Boutique System/DLyah Boutique System.csproj", "DLyah Boutique System/"]

RUN dotnet restore "DLyah Boutique System.sln"

COPY . .

WORKDIR "/src/DLyah Boutique System"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

# --- ADICIONE ESTA LINHA PARA FORÇAR A CÓPIA ---
# (Ajuste o caminho da origem se necessário, mas deve ser relativo à raiz do projeto onde está o Dockerfile)
COPY ["DLyah Boutique System/wwwroot/js/advanced-uploader.js", "/app/wwwroot/js/"]
# -----------------------------------------------

ENTRYPOINT ["dotnet", "DLyah Boutique System.dll"]