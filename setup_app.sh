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
    Username NVARCHAR(50) NOT NULL,
    SenhaHash NVARCHAR(100) NOT NULL
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patios')
BEGIN
  CREATE TABLE Patios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Localizacao NVARCHAR(200) NOT NULL
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Motos')
BEGIN
  CREATE TABLE Motos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Placa NVARCHAR(7) NOT NULL UNIQUE,
    Modelo NVARCHAR(50) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Disponível',
    PatioId INT NULL,
    DataEntrada DATETIME2 NULL,
    CONSTRAINT FK_Motos_Patios FOREIGN KEY (PatioId) REFERENCES Patios(Id) ON DELETE SET NULL
  );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Movimentacoes')
BEGIN
  CREATE TABLE Movimentacoes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MotoId INT NOT NULL,
    PatioId INT NOT NULL,
    DataEntrada DATETIME2 NOT NULL,
    DataSaida DATETIME2 NULL,
    CONSTRAINT FK_Movimentacoes_Motos FOREIGN KEY (MotoId) REFERENCES Motos(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Movimentacoes_Patios FOREIGN KEY (PatioId) REFERENCES Patios(Id) ON DELETE CASCADE
  );
END
SQL

echo "Executando cria_objetos.sql..."
sqlcmd -S "$DbHost,1433" -d "$DbName" -U "$DbUser" -P "$DbPass" -i cria_objetos.sql

echo "[OK] Tabelas do MottuApi.API criadas com sucesso"