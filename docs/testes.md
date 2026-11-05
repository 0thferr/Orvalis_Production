# Testes

## Automatizados

Os testes xUnit cobrem consulta de ordens; histórico com e sem resultados; gravação válida; e-mail inexistente; ordem inexistente; quantidade zero ou acima da ordem; material incompatível; datas antes e depois do período; ciclo zero; e ciclo inferior ao padrão aceito com aviso.

Execução: `dotnet test SequorProduction.sln`. Resultado nesta entrega: 13 aprovados, 0 falhas. Os testes de serviço usam banco EF InMemory descartável para isolamento; a aplicação real usa SQL Server. Também foi verificada uma gravação no SQL Server Express por consulta SQL direta.

## Roteiro manual (Swagger/Postman)

### Consultar ordens

`GET http://localhost:5000/api/orders/GetOrders` — esperado HTTP 200 e envelope `orders` com três ordens.

### Consultar histórico

`GET http://localhost:5000/api/orders/GetProduction?email=teste@sequor.com.br` — esperado HTTP 200 e registros de demonstração. Para usuário sem histórico, esperado `{"productions":[]}`.

### Apontamento válido

`POST http://localhost:5000/api/orders/SetProduction`, `Content-Type: application/json`:

```json
{
  "email":"teste@sequor.com.br", "order":"111", "productionDate":"2026-11-04",
  "productionTime":"10:30:00", "quantity":10, "materialCode":"aaa", "cycleTime":35.5
}
```

Esperado HTTP 200 e `{"status":200,"type":"S","description":"Apontamento realizado com sucesso!"}`.

### Erros de validação

Repita o JSON válido alterando: e-mail para desconhecido; ordem para `999`; quantidade para `0` ou `101`; material para `ccc`; data para `2025-12-31` ou `2027-01-01`; ciclo para `0`. Esperado HTTP 400 e corpo com `status:201`, `type:"E"` e descrição correspondente. Para ciclo positivo abaixo do cadastrado (ex.: `1`), esperado sucesso HTTP 200 e mensagem informativa.

### Persistência SQL

Após um apontamento válido, consulte o histórico pelo e-mail ou execute `SELECT * FROM Production WHERE Email='teste@sequor.com.br' ORDER BY Date DESC;` no SQL Server. O novo registro deve constar.
