#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Estágio de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiando os arquivos do projeto
COPY . .

# Restaurando as dependências e construindo o projeto
RUN dotnet restore
RUN dotnet build -c Debug -o /app/build

# Estágio de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiando o resultado da compilação do estágio anterior
COPY --from=build /app/build .

# Expondo a porta necessária
EXPOSE 8080

# Definindo a variável de ambiente ASPNETCORE_ENVIRONMENT para Development
ENV ASPNETCORE_ENVIRONMENT Development

# Iniciando o aplicativo diretamente usando dotnet run
CMD ["dotnet", "impacta-contatos-api.dll"]