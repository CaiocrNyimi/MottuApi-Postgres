FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "MottuApi/MottuApi.csproj"
RUN dotnet build "MottuApi/MottuApi.csproj" -c Release -o /app/build
RUN dotnet publish "MottuApi/MottuApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=build /app/publish .

COPY --from=build /src/MottuApi /src/MottuApi

COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]