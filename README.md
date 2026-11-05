# Sequor Production

Aplicação de apontamento industrial feita para o desafio Sequor. O projeto tem API ASP.NET Core em C#, persistência SQL Server com EF Core Code First e interface React/Vite que consome a API real.

## Estrutura

```text
backend/SequorProduction.Api/   API, Controllers, Services, Models, DTOs, Data e migration
backend/SequorProduction.Tests/ Testes unitários xUnit das regras
frontend/                      React, Vite e CSS responsivo
database/seed.sql              Carga idempotente para inspeção/execução manual
docs/                           Arquitetura, execução, testes e entrevista
```

## Rotas

- `GET /api/orders/GetOrders`
- `GET /api/orders/GetProduction?email=...`
- `POST /api/orders/SetProduction`

Consulte [docs/como-executar.md](docs/como-executar.md) para pré-requisitos e comandos e [docs/testes.md](docs/testes.md) para exemplos de uso.

## Demonstração

Usuários: `teste@sequor.com.br`, `maria@sequor.com.br`, `joao@sequor.com.br`. Não há senha; este desafio identifica o operador pelo e-mail informado. Ordens: 111 (ABC), 222 (DEF), 333 (GHI). A migration cria o esquema, e o inicializador do EF insere os dados de demonstração de forma idempotente.

## Decisões e limitações

O ciclo enviado é contado no navegador desde a seleção da ordem; o botão só libera após atingir o tempo de ciclo cadastrado. O backend exige valor positivo, permite ciclo abaixo do padrão e inclui aviso. Os testes automatizados de regra usam provedor InMemory isolado, enquanto a persistência da aplicação é sempre SQL Server. A API aplica migrations ao iniciar e não retorna detalhes internos em falhas inesperadas.

## Validação executada neste ambiente

- Restauração NuGet e build da solução .NET 10: concluídos com sucesso.
- Testes xUnit: 13 passaram, 0 falharam.
- Build de produção do React/Vite: concluído com sucesso.
- SQL Server Express local: migration e seed executados; pedidos e históricos consultados pela API.
- POST válido com ciclo abaixo do cadastrado: retornou sucesso e aviso, com gravação confirmada por consulta SQL.
- POST com usuário inexistente: respondeu HTTP 400 e JSON `status: 201`, `type: E`, conforme o contrato.

As versões transitivas vulneráveis inicialmente apontadas pelo NuGet foram atualizadas com referências explícitas para `Microsoft.OpenApi 2.7.5` e `System.Security.Cryptography.Pkcs 9.0.21`; a restauração final terminou sem avisos de vulnerabilidade.
