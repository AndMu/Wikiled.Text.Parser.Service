FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder
SHELL ["powershell"]

ARG BUILD_VERSION="0.0.0.0"
RUN echo $Env:BUILD_VERSION;

WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

ENTRYPOINT ["dotnet", "Wikiled.Text.Parser.Service.dll"]

