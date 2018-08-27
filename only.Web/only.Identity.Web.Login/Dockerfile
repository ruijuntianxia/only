FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 14434
EXPOSE 44361

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY only.Web/only.Identity.Web.Login/only.Identity.Web.Login.csproj only.Web/only.Identity.Web.Login/
RUN dotnet restore only.Web/only.Identity.Web.Login/only.Identity.Web.Login.csproj
COPY . .
WORKDIR /src/only.Web/only.Identity.Web.Login
RUN dotnet build only.Identity.Web.Login.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish only.Identity.Web.Login.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "only.Identity.Web.Login.dll"]
