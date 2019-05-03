FROM mcr.microsoft.com/dotnet/framework/sdk:latest AS builder
SHELL ["powershell", "-NoProfile", "-Command"]

ARG BUILD_VERSION="0.0.0.0"
ARG DOCKER_USER="DockerApp"
ARG DOCKER_PASS=""

RUN echo $Env:DOCKER_PASS;


WORKDIR /source

COPY . .
RUN dotnet publish Wikiled.Text.Parser.Service /p:Version=$Env:BUILD_VERSION --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8
SHELL ["powershell", "-NoProfile", "-Command"]

WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_ENVIRONMENT Production

# Setup network share disk
RUN $password = $Env:DOCKER_PASS | ConvertTo-SecureString -asPlainText -Force; \
	$cred = New-Object System.Management.Automation.PsCredential($Env:DOCKER_USER, $password); \
	New-PSDrive -name D -Root \\192.168.0.140\shared\Docker -Credential $cred -PSProvider filesystem -Persist

ENTRYPOINT ["Wikiled.Text.Parser.Service.exe"]