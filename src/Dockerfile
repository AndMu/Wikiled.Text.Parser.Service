FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder

ARG BUILD_VERSION="0.0.0.0"

WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
SHELL ["powershell", "-NoProfile", "-Command"]

# Create Windows user in the container
RUN net user /add docker
RUN net localgroup Administrators docker /add

# Set it for subsequent commands
USER docker

ARG DOCKER_USER="DockerApp"
ARG DOCKER_PASS=""

RUN echo $Env:DOCKER_PASS;
RUN echo $env:UserName

WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

ENTRYPOINT ["Wikiled.Text.Parser.Service.exe"] 