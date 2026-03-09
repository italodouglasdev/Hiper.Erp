# Hiper.Erp - Sistema de Gestão Empresarial

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Blazor](https://img.shields.io/badge/Blazor-512BD4?style=for-the-badge&logo=blazor)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq)
![xUnit](https://img.shields.io/badge/xUnit-000000?style=for-the-badge&logo=xunit)

O **Hiper.Erp** é um sistema de gestão empresarial (ERP) construído com arquitetura multitenant, desenvolvido como parte do desafio técnico para a vaga de Engenharia de Software Sr. na Hiper. 

Este projeto foi desenhado para demonstrar domínio em arquitetura de software, padrões de projeto, segurança, testes automatizados e integração com mensageria assíncrona.

---

## 🏗️ Arquitetura e Design

O projeto segue os princípios da **Clean Architecture** (Arquitetura Limpa) e **Onion Architecture**, garantindo um alto nível de desacoplamento, testabilidade e facilidade de manutenção. A solução está dividida em 4 camadas principais distribuídas em 20 projetos:

1. **Apresentação (`Hiper.Erp.Apresentacao.Api` e `Hiper.Erp.Apresentacao.Web`)**
   A API RESTful atua como porta de entrada, enquanto o frontend foi desenvolvido em Blazor WebAssembly. A API é responsável apenas por receber requisições, repassar para a camada de aplicação e retornar as respostas HTTP adequadas.

2. **Aplicação (`Hiper.Erp.Aplicacao.*`)**
   Contém as regras de orquestração do negócio. Utiliza DTOs (Data Transfer Objects) para trafegar dados, validadores (Fail-Fast Validation) para garantir a integridade antes do processamento, e interfaces para inverter as dependências.

3. **Domínio (`Hiper.Erp.Dominio.*`)**
   O coração do sistema. Contém as entidades de negócio (Agentes, Produtos, Vendas, etc.) isoladas de qualquer tecnologia de infraestrutura ou banco de dados.

4. **Infraestrutura (`Hiper.Erp.Infraestrutura.*`)**
   Implementa os detalhes técnicos: acesso a dados (Entity Framework Core), cache em memória e integração com mensageria (RabbitMQ).

### Padrões de Projeto (Design Patterns) Utilizados

A solução faz uso intensivo de padrões de projeto consagrados no mercado:

- **Repository Pattern & Unit of Work:** Centraliza a lógica de acesso a dados (`IRepositorioGenerico`) e garante transações atômicas no banco de dados.
- **Dependency Injection (DI):** Todas as dependências são injetadas via construtor, garantindo o baixo acoplamento (Princípio da Inversão de Dependência do SOLID).
- **Factory Method:** A classe `FabricaConexoes` decide em tempo de execução qual contexto de banco de dados instanciar (SQL Server ou PostgreSQL) com base no tenant atual.
- **Middleware Pattern:** O `TenantMiddleware` intercepta as requisições HTTP para extrair o ID do cliente (via Header ou JWT) e configurar o contexto do banco de dados dinamicamente.
- **DTO (Data Transfer Object):** Separação estrita entre o modelo de domínio e o que é exposto nas APIs.
- **Fail-Fast Validation:** Validação de regras de negócio na entrada da aplicação usando classes dedicadas no projeto `Aplicacao.Validadores`.

---

## 🧪 Testes Unitários

A qualidade do código é garantida por uma suíte robusta de testes automatizados. O projeto `Hiper.Erp.Testes.XUnitTestes` conta com **140 testes unitários** passando com sucesso, desenvolvidos utilizando **xUnit** e **Moq**.

A cobertura de testes abrange:
- **Utilitários e Validadores Base:** Testes de algoritmos complexos (CPF, CNPJ, formatação de strings e decimais) utilizando o atributo `[Theory]` para testar múltiplos cenários (dados válidos e inválidos) com o mesmo método.
- **Validadores de DTOs:** Garantia de que as regras de negócio (campos obrigatórios, tamanhos máximos) barram requisições inválidas antes de chegarem aos serviços.
- **Serviços de Aplicação (Mocking):** Testes de orquestração isolando o banco de dados. Utilizando a biblioteca **Moq**, simulamos o comportamento dos repositórios para testar se os serviços (Agentes, Produtos, Vendas) lidam corretamente com sucessos, falhas e exceções, garantindo a integridade das regras de negócio (ex: não permitir a exclusão de um cliente que possui vendas atreladas).

---

## 🐰 Mensageria Assíncrona

Para evitar gargalos de performance e demonstrar a integração com sistemas externos, foi implementada uma fila de mensageria utilizando **RabbitMQ** (`Hiper.Erp.Infraestrutura.Mensageria`).

**Caso de Uso:** No momento em que um novo cliente (Agente) é cadastrado com sucesso no banco de dados, a API publica uma mensagem assíncrona (*fire-and-forget*) na fila `fila-email-boas-vindas`. Isso permite que um serviço externo consuma essa fila e envie o e-mail sem bloquear a resposta HTTP para o usuário.

A implementação conta com resiliência: caso o servidor do RabbitMQ esteja indisponível, o sistema registra um log de erro, mas não impede o cadastro do cliente no ERP.

---

## 🔒 Segurança e Configuração (Deploy)

Uma premissa forte deste projeto é a **segurança**. Nenhuma senha, chave JWT ou Connection String fica exposta em *hardcode* no código-fonte. A arquitetura Multitenant trafega as Connection Strings de forma criptografada (AES) e as descriptografa em tempo de execução.

Para rodar a aplicação localmente ou em produção, é **obrigatório** preencher as configurações no arquivo `appsettings.Development.json` (ou variáveis de ambiente no servidor).

### Campos Obrigatórios no `appsettings.json`

| Seção | Campo | Descrição |
|-------|-------|-----------|
| **Jwt** | `Key` | Chave secreta (mínimo 64 caracteres) usada para assinar e validar os tokens de autenticação. |
| **RabbitMQ** | `UserName` | Usuário de acesso ao servidor RabbitMQ. |
| **RabbitMQ** | `Password` | Senha de acesso ao servidor RabbitMQ. |
| **Seguranca** | `ChaveAesConnectionString` | Chave simétrica usada para descriptografar as Connection Strings dos tenants no `TenantMiddleware`. |
| **ConnectionStringsMock** | `SqlServer` | Connection string completa para o banco SQL Server de desenvolvimento/teste. |
| **ConnectionStringsMock** | `PostgreSQL` | Connection string completa para o banco PostgreSQL de desenvolvimento/teste. |

*Nota: Se esses campos não forem preenchidos, a aplicação lançará uma exceção (`InvalidOperationException`) durante a inicialização, garantindo que o sistema não suba com configurações inseguras.*

---

## 🌐 Ambiente de Produção

A aplicação encontra-se publicada em uma VPS Windows Server 2019 utilizando IIS e está disponível nos seguintes endereços:

| Serviço | URL |
|---------|-----|
| **API do Servidor de Autenticação (Tenants)** | [adm.hiper.italodouglas.dev](https://adm.hiper.italodouglas.dev/swagger) |
| **API REST** | [api.hiper.italodouglas.dev](https://api.hiper.italodouglas.dev/swagger) |
| **Frontend (Retaguarda)** | [retaguarda.hiper.italodouglas.dev](https://retaguarda.hiper.italodouglas.dev/) |
| **RabbitMQ** | [rabbit.hiper.italodouglas.dev](https://rabbit.hiper.italodouglas.dev/) |

---
*Desenvolvido por [Ítalo Douglas](https://italodouglas.dev).*

