# Arquitetura do cash.hub

## 📌 Índice
- [Visão Geral](#visao-geral)
- [Estrutura da Arquitetura](#estrutura-da-arquitetura)
- [Componentes da Arquitetura](#componentes-da-arquitetura)
  - [NGINX (Reverse Proxy)](#nginx-reverse-proxy)
  - [Authentication API](#authentication-api)
  - [Authentication Database (SQL Server)](#authentication-database-sql-server)
  - [Google Pub/Sub (Message Broker para Logs)](#google-pubsub-message-broker-para-logs)
  - [Consumer de Logs](#consumer-de-logs)
  - [TransactionLog Database](#transactionlog-database)
  - [Redis (Cache para Relatórios)](#redis-cache-para-relatorios)
  - [Monitoring Stack](#monitoring-stack-opentelemetry-prometheus-grafana-tempo)
- [Fluxo da Arquitetura](#fluxo-da-arquitetura)
- [Benefícios da Arquitetura](#beneficios-da-arquitetura)
- [Diagrama da Arquitetura](#diagrama-da-arquitetura)
- [Como Rodar as APIs](#como-rodar-as-apis)
- [Importar Collection no Postman](#importar-collection-no-postman)
- [Funcionamento das APIs](#funcionamento-das-apis)
  - [cash.hub.authentication.api](#cashhubauthenticationapi)
  - [cash.hub.register.api](#cashhubregisterapi)
  - [cash.hub.report.api](#cashhubreportapi)
---

<a id="visao-geral"></a>
## 📜 Visao Geral
A arquitetura do **cash.hub** foi projetada para oferecer **segurança, escalabilidade e monitoramento** eficiente dos serviços financeiros. O sistema implementa autenticação JWT, mensageria para logs e uma stack de observabilidade.

<a id="estrutura-da-arquitetura"></a>
## 🏗️ Estrutura da Arquitetura

1. **Adapters**: Responsável pela comunicação entre a API e os serviços externos.
   - **Inbound** (Entrada):
     - `Rest`: Implementação dos endpoints HTTP, incluindo filtros, requests, responses e validações.
   - **Outbound** (Saída):
     - `Repository`: Interface de acesso aos dados.

2. **Application**: Camada que contém a lógica de negócio.
   - `Common`: DTOs, enums e classes de retorno padrão.
   - `Dto`: Definição de inputs e outputs das requisições.
   - `Services`: Implementação dos serviços de negócio.
   - `UseCases`: Casos de uso que orquestram os serviços e regras de negócio.

3. **Domain**: Representação do domínio da aplicação.
   - `Entities`: Modelos de dados utilizados no sistema.
   - `Ports`: Definição das portas de entrada e saída seguindo a Clean Architecture.

4. **Infra**: Configurações e implementações técnicas.
   - `DependencyInjection`: Configuração de injeção de dependências.
   - `EntityFramework`: Implementação do ORM para acesso ao banco de dados.
   - `JwtConfig`: Configuração de autenticação JWT.
   - `Middleware`: Middlewares customizados para tratamento de requisições.
   - `OpenTelemetry`: Monitoramento e rastreamento distribuído.
   - `Rest`: Configurações específicas para APIs REST.
   - `SwaggerConfig`: Configuração da documentação com Swagger.

5. **Migrations**: Gerenciamento de migrações do banco de dados.

### 📂 Estrutura de Pastas

```plaintext
📦 Projeto
├── 📂 Adapters
│   ├── 📂 Inbound (Entrada)
│   │   ├── 📂 Rest
│   │   │   ├── 📂 Common
│   │   │   ├── 📂 Endpoints
│   │   │   ├── 📂 Filter
│   │   │   ├── 📂 Requests
│   │   │   ├── 📂 Responses
│   │   │   ├── 📂 Validators
│   ├── 📂 Outbound (Saída)
│   │   ├── 📂 Repository
├── 📂 Application
│   ├── 📂 Common
│   │   ├── 📂 Dto
│   │   ├── 📂 Enums
│   │   ├── 📄 FactoryBaseReturn.cs
│   ├── 📂 Dto
│   │   ├── 📂 Inputs
│   │   ├── 📂 Outputs
│   ├── 📂 Services
│   ├── 📂 UseCases
├── 📂 Domain
│   ├── 📂 Entities
│   ├── 📂 Ports
├── 📂 Infra
│   ├── 📂 DependencyInjection
│   ├── 📂 EntityFramework
│   ├── 📂 JwtConfig
│   ├── 📂 Middleware
│   ├── 📂 OpenTelemetry
│   ├── 📂 Rest
│   ├── 📂 SwaggerConfig
├── 📂 Migrations
```

<a id="componentes-da-arquitetura"></a>
## 🏗️ Componentes da Arquitetura

<a id="nginx-reverse-proxy"></a>
### 🔹 **NGINX (Reverse Proxy)**
- Atua como gateway de entrada, recebendo requisições HTTPs.
- Encaminha chamadas para os serviços adequados.
- Implementa Rate Limiting para evitar abusos.
- Obs: NGINX não foi colocado no compose mas está no desenho da arquitetura original

<a id="authentication-api"></a>
### 🔹 **Authentication API**
- Responsável por autenticação e geração de tokens JWT.
- Conecta-se ao **Authentication Database** (SQL Server) para validar usuários.
- Tokens possuem expiração de **60 minutos**.
- No futuro, pode ser substituído por **Keycloak** para uma autenticação mais robusta.

<a id="authentication-database-sql-server"></a>
### 🔹 **Authentication Database (SQL Server)**
- Armazena credenciais de usuários de forma segura.
- Senhas são **hashadas** para maior proteção.

<a id="google-pubsub-message-broker-para-logs"></a>
### 🔹 **Google Pub/Sub (Message Broker para Logs)**
- Processa logs de eventos de forma assíncrona.
- Evita sobrecarga direta no banco de dados.
- Envia logs para o **Consumer** processá-los antes de armazenar.
- Obs: Devido ao curto tempo, não consegui implementar essa solução.

<a id="consumer-de-logs"></a>
### 🔹 **Consumer de Logs**
- Processa mensagens do **Google Pub/Sub**.
- Insere logs no **TransactionLog Database**.

<a id="transactionlog-database"></a>
### 🔹 **TransactionLog Database**
- Armazena logs processados.
- Facilita consultas e auditorias de eventos.

<a id="redis-cache-para-relatorios"></a>
### 🔹 **Redis (Cache para Relatorios)**
- Utilizado para otimizar a consulta de relatórios na **CashHub Report API**.
- Reduz a carga no banco de dados armazenando consultas frequentemente acessadas.

<a id="monitoring-stack-opentelemetry-prometheus-grafana-tempo"></a>
### 🔹 **Monitoring Stack (OpenTelemetry, Prometheus, Grafana Tempo)**
- **OpenTelemetry** coleta métricas e traces.
- **Prometheus** armazena e processa métricas de desempenho.
- **Grafana Tempo** exibe dashboards para análise de traces e alertas.
- Inicialmente, a configuração estava no Docker Compose, mas automatizá-la se mostrou complexa. Para evitar perda de tempo, optei por não incluí-la no ambiente. No entanto, vale ressaltar que as APIs já estão preparadas para exportar traces e métricas, uma exigência essencial em um ambiente de microsserviços.

<a id="fluxo-da-arquitetura"></a>
## 🔗 Fluxo da Arquitetura
1. O **Caixa** precisa fazer um lançamento de **débito ou crédito**.
2. Para acessar a **Cash Register API**, ele precisa se autenticar.
3. O caixa envia credenciais para a **Authentication API**, que valida no **Authentication Database** e gera um **token JWT**.
4. O token JWT é usado para acessar a **Cash Register API**, garantindo que apenas usuários autenticados possam fazer lançamentos.
5. Logs da autenticação e transações são enviados para **Google Pub/Sub**.
6. O **Consumer** processa logs e armazena no **TransactionLog Database**.
7. O **Monitoring Stack** analisa métricas e traces em tempo real.
8. O **Redis Cache** é utilizado para armazenar consultas de relatórios frequentemente acessadas pela **CashHub Report API**, reduzindo a carga no banco.

📌 *Nota:* Houve a intenção de utilizar **Keycloak** para autenticação, mas devido ao curto prazo, foi implementado JWT manualmente. Futuramente, a adoção do Keycloak pode centralizar e aprimorar a autenticação.

<a id="beneficios-da-arquitetura"></a>
## 📌 Beneficios da Arquitetura
✅ Maior segurança com autenticação JWT e senhas hashadas.
✅ Desacoplamento de logs através de Google Pub/Sub e Consumer.
✅ Monitoramento completo com OpenTelemetry, Prometheus e Grafana Tempo.
✅ **Cache de relatórios com Redis**, reduzindo consultas repetitivas ao banco de dados.

<a id="diagrama-da-arquitetura"></a>
## 🖼️ Diagrama da Arquitetura
![Arquitetura cash.hub](Images/Cash_Hub_Arquitetura.jpg)

<a id="como-rodar-as-apis"></a>
## 🚀 Como Rodar as APIs

### 📌 Pré-requisitos
Antes de rodar as APIs, certifique-se de ter instalado:
- **Docker** (para rodar os containers de SQL Server e Redis)
- **.NET 8 SDK** (para compilar e executar as APIs)

1. **Clonar o repositório**:
   ```sh
   git clone https://github.com/kadubezas/cash-hub.git
   cd cash-hub
   ```

2. **Subir os containers do SQL Server e Redis via Docker Compose**:
   - Os arquivos do **docker-compose** estão na pasta `Config`.
   ```sh
   docker-compose -f Config/docker-compose.yml up -d
   ```

3. **Executar as APIs**:
   ```sh
   dotnet run --project cash.hub/src/cash.hub.authentication.api
   dotnet run --project cash.hub/src/cash.hub.register.api
   dotnet run --project cash.hub/src/cash.hub.report.api
   ```

4. **Acessar os endpoints** via Swagger:
   - `http://localhost:5066/swagger`
   - `http://localhost:5231/swagger`
   - `http://localhost:5219/swagger`

<a id="importar-collection-no-postman"></a>
## 🛠️ Importar Collection no Postman

A collection do Postman está disponível na pasta `PostmanCollection`. Para facilitar os testes das APIs, siga os passos abaixo: Para facilitar os testes das APIs, você pode importar a collection do **Postman** seguindo estes passos:

1. **Abrir o Postman**
2. **Clicar em "Import"** (no canto superior esquerdo)
3. **Selecionar a opção "File"** e escolher o arquivo JSON da collection
4. **Clicar em "Import"** para carregar as rotas das APIs
5. **Executar as requisições** e validar as respostas


<a id="funcionamento-das-apis"></a>
## 🛠️ Funcionamento das APIs

<a id="cashhubauthenticationapi"></a>
### 🔐 cash.hub.authentication.api

📌 **Usuário Padrão:**
Por padrão, a API possui um usuário inicial para testes:
- **userName:** `admin`
- **password:** `admin12345`

A API de autenticação é responsável por gerar tokens JWT e gerenciar usuários.

#### 📌 Endpoints principais:

1. **Autenticação (Gerar Token JWT)**
   - **Endpoint:** `POST /authentication/authenticate`
   - **Request Body:**
     ```json
     {
       "userName": "string",
       "password": "string"
     }
     ```
   - **Resposta(201):**
     ```json
     {
       "token": "eyJhbGciOiJIUzI1NiIsInR...",
       "Expiration": "2025-03-14T13:48:24.0361437Z"
     }
     ```

2. **Registro de Usuário**
   - **Endpoint:** `POST /authentication/user/register`
   - **Request Body:**
     ```json
     {
       "userName": "string",
       "password": "string"
     }
     ```
   - **Resposta(200):**
     ```json
     {
       "message": "Usuário registrado com sucesso"
     }
     ```
#### 📌 Tratamento de Erros
A API retorna erros padronizados para facilitar o diagnóstico de falhas:

- **Erro 400 (Bad Request)**: Requisição inválida ou parâmetros incorretos.
- **Erro 500 (Internal Server Error)**: Erro inesperado no servidor.

📌 **Exemplo de resposta de erro:**
```json
{
  "code": 0,
  "message": "string",
  "errors": [
    {
      "field": "string",
      "message": "string"
    }
  ]
}
```

📌 **Os tokens JWT gerados têm expiração de 60 minutos e são utilizados para autenticação nas demais APIs.** 🚀


<a id="cashhubregisterapi"></a>
### 🔄 cash.hub.register.api

📌 **Autenticação:**
Todas as requisições para esta API devem incluir um token JWT no cabeçalho `Authorization`.

**Exemplo de cabeçalho:**
```http
Authorization: Bearer <seu_token_jwt>
```

A API de transações é responsável por registrar transações financeiras.

#### 📌 Endpoints principais:

1. **Registrar Transação**
   - **Endpoint:** `POST /cash/hub/v1/transaction/register`
   - **Request Body:**
     ```json
     {
       "type": 0,
       "cashRegisterId": 0,
       "amount": 0,
       "paymentMethod": "string",
       "installments": 0
     }
     ```
   - **Resposta (201 - Created):**
     ```json
     {
       "transactionId": "7b4516f2-9731-45b3-b476-3d87b9f6fafe",
       "status": 0,
       "createdAt": "2023-12-18T12:56:00.947Z"
     }
     ```

#### 📌 Tratamento de Erros
A API retorna erros padronizados para facilitar o diagnóstico de falhas:

- **Erro 400 (Bad Request)**: Requisição inválida ou parâmetros incorretos.
- **Erro 500 (Internal Server Error)**: Erro inesperado no servidor.

📌 **Exemplo de resposta de erro:**
```json
{
  "code": 0,
  "message": "string",
  "errors": [
    {
      "field": "string",
      "message": "string"
    }
  ]
}
```
<a id="cashhubreportapi"></a>
### 📊 cash.hub.report.api

📌 **Autenticação:** Todas as requisições para esta API devem incluir um token JWT no cabeçalho `Authorization`.

**Exemplo de cabeçalho:**

```http
Authorization: Bearer <seu_token_jwt>
```

A API de relatórios permite a consulta de transações.

#### 📌 Endpoints principais:

1. **Consultar Transações**
   - **Endpoint:** `GET /cash/v1/transactions`
   - **Parâmetros Query:**
     - `date` (string, formato `yyyy-MM-dd`, Obrigatório): Data da transação.
     - `page` (integer): Página da consulta.
     - `pageSize` (integer): Quantidade de registros por página.
   - **Resposta (200 - OK):**
     ```json
     {
       "transactions": [
         {
           "transactionId": "7b4516f2-9731-45b3-b476-3d87b9f6fafe",
           "amount": 0,
           "status": 0,
           "createdAt": "2023-12-18T12:56:00.132Z"
         }
       ],
       "pagination": {
         "page": 1,
         "pageSize": 0,
         "totalPages": 0
       }
     }
     ```

#### 📌 Tratamento de Erros

A API retorna erros padronizados para facilitar o diagnóstico de falhas:

- **Erro 400 (Bad Request)**: Requisição inválida ou parâmetros incorretos.
- **Erro 500 (Internal Server Error)**: Erro inesperado no servidor.

📌 **Exemplo de resposta de erro:**

```json
{
  "code": 0,
  "message": "string",
  "errors": [
    {
      "field": "string",
      "message": "string"
    }
  ]
}
```
