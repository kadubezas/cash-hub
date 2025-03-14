# Arquitetura do cash.hub

## 📌 Índice
- [Visão Geral](#visao-geral)
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

---

## 📜 Visao Geral
A arquitetura do **cash.hub** foi projetada para oferecer **segurança, escalabilidade e monitoramento** eficiente dos serviços financeiros. O sistema implementa autenticação JWT, mensageria para logs e uma stack de observabilidade.

## 🏗️ Componentes da Arquitetura

### 🔹 **NGINX (Reverse Proxy)**
- Atua como gateway de entrada, recebendo requisições HTTPs.
- Encaminha chamadas para os serviços adequados.
- Implementa Rate Limiting para evitar abusos.

### 🔹 **Authentication API**
- Responsável por autenticação e geração de tokens JWT.
- Conecta-se ao **Authentication Database** (SQL Server) para validar usuários.
- Tokens possuem expiração de **60 minutos**.
- No futuro, pode ser substituído por **Keycloak** para uma autenticação mais robusta.

### 🔹 **Authentication Database (SQL Server)**
- Armazena credenciais de usuários de forma segura.
- Senhas são **hashadas** para maior proteção.

### 🔹 **Google Pub/Sub (Message Broker para Logs)**
- Processa logs de eventos de forma assíncrona.
- Evita sobrecarga direta no banco de dados.
- Envia logs para o **Consumer** processá-los antes de armazenar.

### 🔹 **Consumer de Logs**
- Processa mensagens do **Google Pub/Sub**.
- Insere logs no **TransactionLog Database**.

### 🔹 **TransactionLog Database**
- Armazena logs processados.
- Facilita consultas e auditorias de eventos.

### 🔹 **Redis (Cache para Relatorios)**
- Utilizado para otimizar a consulta de relatórios na **CashHub Report API**.
- Reduz a carga no banco de dados armazenando consultas frequentemente acessadas.

### 🔹 **Monitoring Stack (OpenTelemetry, Prometheus, Grafana Tempo)**
- **OpenTelemetry** coleta métricas e traces.
- **Prometheus** armazena e processa métricas de desempenho.
- **Grafana Tempo** exibe dashboards para análise de traces e alertas.

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

## 📌 Beneficios da Arquitetura
✅ Maior segurança com autenticação JWT e senhas hashadas.
✅ Desacoplamento de logs através de Google Pub/Sub e Consumer.
✅ Monitoramento completo com OpenTelemetry, Prometheus e Grafana Tempo.
✅ **Cache de relatórios com Redis**, reduzindo consultas repetitivas ao banco de dados.

## 🖼️ Diagrama da Arquitetura
![Arquitetura cash.hub](Images/Cash_Hub_Arquitetura.jpg)

📌 **Para mais informações, consulte a documentação de cada serviço.** 🚀

