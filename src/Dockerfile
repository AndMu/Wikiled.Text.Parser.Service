FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder

ARG BUILD_VERSION="0.0.0.0"

WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8

# install vc_redist
ADD https://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x64.exe /vc_redist.x64.exe
RUN C:\vc_redist.x64.exe /quiet /install

WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["Wikiled.Text.Parser.Service.exe"] 