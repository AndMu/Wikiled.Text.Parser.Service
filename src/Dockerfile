FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder

ARG BUILD_VERSION="0.0.0.0"

WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8

WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

ENTRYPOINT ["Wikiled.Text.Parser.Service.exe"] 