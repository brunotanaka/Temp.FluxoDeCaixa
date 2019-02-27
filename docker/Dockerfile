# Build ###################################################################
FROM microsoft/aspnetcore-build:2.0 AS build-env

ARG projeto
ARG repositorio
ARG pathProjeto

RUN apt-get update && apt-get install -y git

RUN git clone $repositorio
WORKDIR $pathProjeto

WORKDIR $projeto

RUN mv $projeto.csproj main.csproj

RUN dotnet restore
RUN dotnet publish -c Release -o out

# # Aplicação ###################################################################
FROM microsoft/aspnetcore:2.0

ARG projeto
ARG pathProjeto

COPY --from=build-env /$pathProjeto/$projeto/out .

ENTRYPOINT ["dotnet","main.dll"]