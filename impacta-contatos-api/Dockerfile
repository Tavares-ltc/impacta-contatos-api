#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Est�gio de constru��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiando os arquivos do projeto
COPY . .

# Restaurando as depend�ncias e construindo o projeto
RUN dotnet restore
RUN dotnet build -c Debug -o /app/build

# Est�gio de execu��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiando o resultado da compila��o do est�gio anterior
COPY --from=build /app/build .

# Expondo a porta necess�ria
EXPOSE 8080

# Definindo a vari�vel de ambiente ASPNETCORE_ENVIRONMENT para Development
ENV ASPNETCORE_ENVIRONMENT Development

# Iniciando o aplicativo diretamente usando dotnet run
CMD ["dotnet", "impacta-contatos-api.dll"]