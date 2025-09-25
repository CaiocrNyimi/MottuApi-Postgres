
# 🏍️ API de Gestão de Motos:  Henzo Puchetti - RM555179 e Luann Domingos Mariano - RM558548

---

## 📌 Descrição

Com uma arquitetura simples e eficiente para facilitar manutenção e escalabilidade, desenvolvemos uma:
API RESTful para gerenciamento de motos, pátios e suas movimentações, desenvolvida em ASP.NET Core com Entity Framework Core e banco Oracle. Permite operações CRUD completas, consultas parametrizadas de moto por ID e placa, e oferece documentação automática via Swagger.

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
  `https://localhost:{porta}/swagger` (interface Swagger para testes).
  *EU RODEI NA URL* - `http://localhost:5248/swagger`
---

