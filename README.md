# Orvalis Production

Aplicação de apontamento industrial. O projeto tem API ASP.NET Core em C#, persistência SQL Server com EF Core Code First e interface React/Vite que consome a API real.

## Estrutura

```text
backend/OrvalisProduction.Api/   API, regras de negócio, modelos e migrations
backend/OrvalisProduction.Tests/ Testes xUnit
frontend/                      React, Vite e CSS responsivo
database/seed.sql              Carga manual de demonstração
docs/                          Arquitetura, execução e testes
```

## Rotas

- `GET /api/orders/GetOrders`
- `GET /api/orders/GetProduction?email=...`
- `POST /api/orders/SetProduction`

Consulte [docs/como-executar.md](docs/como-executar.md) para pré-requisitos e comandos e [docs/testes.md](docs/testes.md) para exemplos de uso.

## Demonstração

Usuários de demonstração: `teste@orvalis.example`, `maria@orvalis.example`, `joao@orvalis.example`. O e-mail informado identifica o operador. Ordens de demonstração: 111 (ABC), 222 (DEF), 333 (GHI). As migrations criam o esquema, e o inicializador do EF popula as tabelas quando o banco está vazio.

## Decisões e limitações

O navegador mede o tempo de ciclo a partir da seleção da ordem e libera o envio quando o tempo cadastrado é atingido. A API exige um tempo positivo e retorna um aviso quando o valor informado está abaixo do ciclo cadastrado. As migrations são aplicadas na inicialização da API.

## Validação

Execute `dotnet test OrvalisProduction.sln` na raiz e `pnpm build` no diretório `frontend` após instalar as dependências. O roteiro de validação com SQL Server está em [docs/testes.md](docs/testes.md).
