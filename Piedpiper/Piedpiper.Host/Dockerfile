FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Piedpiper.Host/Piedpiper.Host.csproj", "Piedpiper.Host/"]
RUN dotnet restore "Piedpiper.Host/Piedpiper.Host.csproj"
COPY . .
WORKDIR "/src/Piedpiper.Host"
RUN dotnet build "Piedpiper.Host.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Piedpiper.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Piedpiper.Host.dll"]