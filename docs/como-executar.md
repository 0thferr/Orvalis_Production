# Como executar

## Pré-requisitos

- .NET 10 SDK.
- SQL Server Express ou Developer, com serviço iniciado.
- Node.js 22+ e pnpm.
- SQL Server acessível; configuração padrão: `localhost\SQLEXPRESS` com autenticação integrada.

## Banco e API

1. Por padrão, a API usa o banco local `OrvalisProduction`. Para reutilizar um banco existente, aponte `ConnectionStrings__Production` para ele antes de iniciar a API. Os e-mails já cadastrados e os históricos continuam associados aos registros existentes.
2. Na raiz, execute `dotnet restore OrvalisProduction.sln`.
3. Execute `dotnet tool install --global dotnet-ef` (se ainda não tiver a ferramenta).
4. `dotnet ef database update --project backend/OrvalisProduction.Api` aplica migrations. Na inicialização a API também aplica migrations e popula os dados iniciais.
5. Inicie com `dotnet run --project backend/OrvalisProduction.Api --urls http://localhost:5000`.
6. Swagger: `http://localhost:5000/swagger`.

A carga automática de demonstração é aplicada somente quando as tabelas da aplicação estão vazias. Para executar `database/seed.sql` manualmente, selecione explicitamente um banco de demonstração com as migrations aplicadas, por exemplo `sqlcmd -S localhost\SQLEXPRESS -E -C -d OrvalisProduction -i database/seed.sql`.

## Front-end

1. Copie `frontend/.env.example` para `frontend/.env` e confira `VITE_API_URL`.
2. Execute `cd frontend`, `pnpm install --frozen-lockfile` e `pnpm dev`.
3. Acesse `http://localhost:5173`.

Para gerar o build do front-end, execute `pnpm build` no mesmo diretório.

## Testes

Na raiz: `dotnet test OrvalisProduction.sln`. A suíte de regras usa o provedor EF InMemory isolado por teste; o ambiente de produção permanece SQL Server. Para validar persistência real, inicie SQL Server, suba a API e consulte o histórico após um POST.
