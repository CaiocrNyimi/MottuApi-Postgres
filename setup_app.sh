#!/usr/bin/env bash
set -euo pipefail

# Parâmetros POSICIONAIS obrigatórios
DbHost="${1:?DbHost ausente}"
DbName="${2:?DbName ausente}"
DbUser="${3:?DbUser ausente}"
DbPass="${4:?DbPass ausente}"

# ================================
# Verificação e instalação do sqlcmd
# ================================
if ! command -v sqlcmd &> /dev/null; then
  echo "sqlcmd não encontrado. Instalando..."
  curl -sSL https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
  curl -sSL https://packages.microsoft.com/config/ubuntu/20.04/prod.list | sudo tee /etc/apt/sources.list.d/mssql-release.list
  sudo apt-get update
  sudo ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools unixodbc-dev
  export PATH="$PATH:/opt/mssql-tools/bin:/usr/bin"
  echo 'export PATH="$PATH:/opt/mssql-tools/bin:/usr/bin"' >> ~/.bashrc
else
  echo "sqlcmd já está instalado."
fi

# ================================
# Criação das tabelas do MottuApi.API (SQL Server)
# ================================
echo "Criando Script de Banco..."
cat > cria_objetos.sql <<'SQL'
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
  CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Senha NVARCHAR(200) NOT NULL,
    TipoUsuario NVARCHAR(100) NOT NULL
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Motos')
BEGIN
  CREATE TABLE Motos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Modelo NVARCHAR(100) NOT NULL,
    Placa NVARCHAR(20) NOT NULL UNIQUE,
    Ano INT NOT NULL,
    Status NVARCHAR(50),
    UsuarioId INT FOREIGN KEY REFERENCES Usuarios(Id) ON DELETE SET NULL
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patios')
BEGIN
  CREATE TABLE Patios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Endereco NVARCHAR(200),
    Capacidade INT
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movimentacoes')
BEGIN
  CREATE TABLE Movimentacoes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MotoId INT FOREIGN KEY REFERENCES Motos(Id) ON DELETE CASCADE,
    PatioId INT FOREIGN KEY REFERENCES Patios(Id) ON DELETE CASCADE,
    DataEntrada DATETIME2 NOT NULL,
    DataSaida DATETIME2
  );
END
SQL

echo "Executando cria_objetos.sql..."
sqlcmd -S "$DbHost,1433" -d "$DbName" -U "$DbUser" -P "$DbPass" -i cria_objetos.sql

echo "[OK] Tabelas do MottuApi.API criadas com sucesso"

# ================================
# Provisionamento da WebApp .NET (opcional para CI/CD)
# ================================
rg="rg-azurewebapp"
location="brazilsouth"
plan="planMottuApi"
app="webapp-mottuapicloud"
runtime="dotnet:8"
sku="F1"

echo "Criando Grupo de Recursos..."
az group create --name "$rg" --location "$location" 1>/dev/null

echo "Criando Plano de Serviço..."
az appservice plan create --name "$plan" --resource-group "$rg" --location "$location" --sku "$sku" 1>/dev/null

echo "Criando Serviço de Aplicativo..."
az webapp create --resource-group "$rg" --plan "$plan" --runtime "$runtime" --name "$app" 1>/dev/null

echo "Habilitando Logs do Serviço de Aplicativo..."
az webapp log config \
  --resource-group "$rg" \
  --name "$app" \
  --application-logging filesystem \
  --web-server-logging filesystem \
  --level information \
  --detailed-error-messages true \
  --failed-request-tracing true 1>/dev/null

echo "[OK] WebApp MottuApi.API provisionado e banco configurado"