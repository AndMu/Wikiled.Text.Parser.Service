FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder
SHELL ["powershell"]

RUN $BUILD_VERSION=echo [Environment]::GetEnvironmentVariable('BUILD_VERSION'); echo $BUILD_VERSION;
RUN echo ${BUILD_VERSION}
WORKDIR /source

COPY . .
RUN dotnet publish /p:Version=$BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

ENTRYPOINT ["dotnet", "Wikiled.Text.Parser.Service.dll"]

