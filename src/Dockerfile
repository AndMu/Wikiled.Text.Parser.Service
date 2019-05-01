FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder
SHELL ["powershell"]

ARG BUILD_VERSION="0.0.0.0"
ARG DOCKER_USER="DockerApp"
ARG DOCKER_PASS=""

RUN echo $Env:BUILD_VERSION;
RUN echo $Env:DOCKER_USER;
RUN echo $Env:DOCKER_PASS;

WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

RUN net use d: \\192.168.0.140\shared\Docker /user:$Env:DOCKER_USER $Env:DOCKER_PASS /persistent:Yes
ENTRYPOINT ["Wikiled.Text.Parser.Service.exe"]

