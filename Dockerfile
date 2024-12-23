# build with the SDK image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
COPY . .
WORKDIR /src/CJansson
RUN dotnet restore -a $TARGETARCH
RUN dotnet publish -a $TARGETARCH --no-restore -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 
WORKDIR /app 
EXPOSE 8080
COPY --from=build /app/publish .
USER $APP_UID 
ENTRYPOINT ["./CJansson"] 