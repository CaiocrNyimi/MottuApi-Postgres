-- Usuários
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
  CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    SenhaHash NVARCHAR(100) NOT NULL
  );
END

-- Pátios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patios')
BEGIN
  CREATE TABLE Patios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Localizacao NVARCHAR(200) NOT NULL
  );
END

-- Motos
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

-- Movimentações
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
