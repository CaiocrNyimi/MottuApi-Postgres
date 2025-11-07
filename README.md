# API de Gestão de Motos:  

## Integrantes:

- Henzo Puchetti - RM555179
- Luann Domingos Mariano - RM558548
- Caio Cesar Rosa Nyimi - RM556331

---

## Descrição

Esta API RESTful foi desenvolvida para gerenciar o fluxo de motos em pátios de estacionamento, permitindo controle preciso sobre entrada, saída e movimentações. O sistema é ideal para empresas que possuem operações envolvendo frotas de motocicletas.

A solução foi construída com ASP.NET Core e Entity Framework Core, utilizando banco de dados SQLServer. A arquitetura foi projetada com foco em escalabilidade, separação de responsabilidades e boas práticas RESTful, como:

- CRUD completo para Moto, Pátio e Movimentação;
- Paginação nas listagens;
- HATEOAS nos endpoints principais;
- Documentação via Swagger;
- Camada de serviços para encapsular regras de negócio.

---

## Modelagem e Domínio

O sistema é composto por três entidades principais:

- **Moto**: representa uma moto cadastrada no sistema. Cada moto está associada a um pátio via chave estrangeira PatioId, garantindo integridade relacional;

- **Pátio**: local físico onde motos são estacionadas. Pode conter várias motos e movimentações;

- **Movimentação**: registra a entrada e saída de uma moto em um pátio, com data/hora e vínculo entre as entidades.

## Relacionamentos

- Uma **Moto** pertence a um único **Pátio**;
- Um **Pátio** pode conter várias **Motos** e **Movimentações**;
- Uma **Movimentação** está vinculada a uma **Moto** e a um **Pátio**.

---

## Arquitetura

A API segue o padrão de camadas:

- **Controllers**: responsáveis por receber requisições HTTP e transmitir para os services;
- **Services**: encapsulam regras de negócio e acesso ao banco;
- **Models**: representam as entidades do domínio;
- **DbContext**: gerencia o mapeamento com o banco Oracle.

---

## Rotas da API

### (Motos)

| Método | Endpoint               | Descrição                        | Códigos HTTP Esperados                         |
|--------|------------------------|----------------------------------|------------------------------------------------|
| GET    | /api/motos             | Retorna todas as motos           | 200 OK                                         |
| GET    | /api/motos/{id}        | Retorna moto por ID              | 200 OK, 404 Not Found                          |
| GET    | /api/motos/search      | Retorna moto pela placa (query)  | 200 OK, 404 Not Found                          |
| POST   | /api/motos             | Cria uma nova moto               | 201 Created, 400 Bad Request                   |
| PUT    | /api/motos/{id}        | Atualiza uma moto existente      | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/motos/{id}        | Exclui uma moto por ID           | 204 No Content, 404 Not Found                  |

### (Patios)

| Método | Endpoint               | Descrição                        | Códigos HTTP Esperados                         |
|--------|------------------------|----------------------------------|------------------------------------------------|
| GET    | /api/patios            | Retorna todos os pátios          | 200 OK                                         |
| GET    | /api/patios/{id}       | Retorna pátio por ID             | 200 OK, 404 Not Found                          |
| POST   | /api/patios            | Cria um novo pátio               | 201 Created, 400 Bad Request                   |
| PUT    | /api/patios/{id}       | Atualiza um pátio existente      | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/patios/{id}       | Exclui um pátio por ID           | 204 No Content, 404 Not Found                  |

### (Movimentacoes)

| Método | Endpoint               | Descrição                        | Códigos HTTP Esperados                         |
|--------|------------------------|----------------------------------|------------------------------------------------|
| GET    | /api/movimentacoes     | Retorna todas movimentações      | 200 OK                                         |
| GET    | /api/movimentacoes/{id}| Retorna movimentação por ID      | 200 OK, 404 Not Found                          |
| POST   | /api/movimentacoes     | Cria nova movimentação           | 201 Created, 400 Bad Request                   |
| PUT    | /api/movimentacoes/{id}| Atualiza movimentação existente  | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | /api/movimentacoes/{id}| Exclui movimentação por ID       | 204 No Content, 404 Not Found                  |

---

## 🚀 Instalação e Execução

### ✅ Pré-requisitos

- .NET 7 SDK  
- Oracle Database (local ou remoto)  
- Visual Studio 2022 / VS Code

### 🔧 Configuração do Banco de Dados

No arquivo `appsettings.json`, configure a string de conexão Oracle:

```json
"ConnectionStrings": {
  "OracleConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_host:porta/servico"
}
```

Execute as migrations para criar as tabelas no banco:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### ▶️ Executando a Aplicação

- Abra a solução no Visual Studio ou VS Code.  
- Configure `MottuApi` como projeto de inicialização.  
- Execute (`Ctrl + F5` ou `dotnet run`).  
- Acesse a API via navegador ou Postman em:  
  `https://localhost:5000/swagger` (interface Swagger para testes).
  
---

## Testes Automatizados

Para rodar os testes unitários:
```bash
dotnet test
```

---

### 📦 Exemplos de Requisições JSON

Abaixo estão exemplos de objetos JSON utilizados nas principais rotas da API:

🛵 Motos
```json
{
  "placa": "ABC1234",
  "modelo": "Honda CG 160",
  "status": "Disponível",
  "patioId": 1,
  "dataEntrada": "2025-10-01T08:00:00Z",
  "dataSaida": null
}
```
- placa: Identificador da moto
- modelo: Modelo da moto
- status: Situação atual (ex: Disponível, Em manutenção, Alugada)
- patioId: Id do pátio onde está localizada
- dataEntrada: Data e hora de entrada no pátio
- dataSaida: Data e hora de saída (pode ser null se ainda estiver no pátio)

🏢 Pátios
```json
{
  "nome": "Pátio Central",
  "localizacao": "Rua das Motos, 123 - São Paulo"
}
```
- nome: Nome do pátio
- localizacao: Endereço físico do pátio

🔄 Movimentações
```json
{
  "motoId": 1,
  "patioId": 1,
  "dataEntrada": "2025-10-01T08:30:00Z",
  "dataSaida": null
}
```
- motoId: ID da moto envolvida na movimentação
- patioId: ID do pátio de destino
- dataEntrada: Data e hora de entrada
- dataSaida: Data e hora de saída (pode ser null se ainda estiver no pátio)
