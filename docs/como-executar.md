# Como executar

## Pré-requisitos

- .NET 10 SDK.
- SQL Server Express ou Developer, com serviço iniciado.
- Node.js 20+ e npm.
- SQL Server acessível; configuração padrão: `localhost\SQLEXPRESS` com autenticação integrada.

## Banco e API

1. Ajuste `ConnectionStrings:Production` em `backend/SequorProduction.Api/appsettings.json` ou defina `ConnectionStrings__Production` no ambiente. Não commite credenciais.
2. Na raiz, execute `dotnet restore SequorProduction.sln`.
3. Execute `dotnet tool install --global dotnet-ef` (se ainda não tiver a ferramenta).
4. `dotnet ef database update --project backend/SequorProduction.Api` aplica migrations. Na inicialização a API também aplica migrations e popula os dados iniciais.
5. Inicie com `dotnet run --project backend/SequorProduction.Api --urls http://localhost:5000`.
6. Swagger: `http://localhost:5000/swagger`.

O script `database/seed.sql` é alternativa de inspeção/execução manual depois de criar o banco e aplicar a migration; a inicialização automática é a opção recomendada.

## Front-end

1. Copie `frontend/.env.example` para `frontend/.env` e confira `VITE_API_URL`.
2. `cd frontend`, `npm install` e `npm run dev`.
3. Acesse `http://localhost:5173`.

## Testes

Na raiz: `dotnet test SequorProduction.sln`. A suíte de regras usa o provedor EF InMemory isolado por teste; o ambiente de produção permanece SQL Server. Para validar persistência real, inicie SQL Server, suba a API e consulte o histórico após um POST.
