IF NOT EXISTS (SELECT 1 FROM [User] WHERE Email = 'teste@orvalis.example')
    INSERT INTO [User](Email, Name, InitialDate, EndDate) VALUES ('teste@orvalis.example', N'Usuário de Teste', '2026-01-01T00:00:00', '2026-12-31T23:59:59');
IF NOT EXISTS (SELECT 1 FROM [User] WHERE Email = 'maria@orvalis.example')
    INSERT INTO [User](Email, Name, InitialDate, EndDate) VALUES ('maria@orvalis.example', N'Maria Oliveira', '2026-01-01T00:00:00', '2026-12-31T23:59:59');
IF NOT EXISTS (SELECT 1 FROM [User] WHERE Email = 'joao@orvalis.example')
    INSERT INTO [User](Email, Name, InitialDate, EndDate) VALUES ('joao@orvalis.example', N'João Santos', '2026-06-01T00:00:00', '2026-11-30T23:59:59');
IF NOT EXISTS (SELECT 1 FROM Product WHERE ProductCode = 'abc') INSERT INTO Product VALUES ('abc', N'Produto ABC', '/images/product-abc.svg', 30.30);
IF NOT EXISTS (SELECT 1 FROM Product WHERE ProductCode = 'def') INSERT INTO Product VALUES ('def', N'Produto DEF', '/images/product-def.svg', 45.50);
IF NOT EXISTS (SELECT 1 FROM Product WHERE ProductCode = 'ghi') INSERT INTO Product VALUES ('ghi', N'Produto GHI', '/images/product-ghi.svg', 60.00);
IF NOT EXISTS (SELECT 1 FROM Material WHERE MaterialCode = 'aaa') INSERT INTO Material VALUES ('aaa', N'Material A - Alumínio');
IF NOT EXISTS (SELECT 1 FROM Material WHERE MaterialCode = 'bbb') INSERT INTO Material VALUES ('bbb', N'Material B - Aço');
IF NOT EXISTS (SELECT 1 FROM Material WHERE MaterialCode = 'ccc') INSERT INTO Material VALUES ('ccc', N'Material C - Plástico');
IF NOT EXISTS (SELECT 1 FROM Material WHERE MaterialCode = 'ddd') INSERT INTO Material VALUES ('ddd', N'Material D - Componente eletrônico');
IF NOT EXISTS (SELECT 1 FROM [Order] WHERE [Order] = '111') INSERT INTO [Order] VALUES ('111', 100, 'abc');
IF NOT EXISTS (SELECT 1 FROM [Order] WHERE [Order] = '222') INSERT INTO [Order] VALUES ('222', 200, 'def');
IF NOT EXISTS (SELECT 1 FROM [Order] WHERE [Order] = '333') INSERT INTO [Order] VALUES ('333', 50, 'ghi');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='abc' AND MaterialCode='aaa') INSERT INTO ProductMaterial VALUES ('abc','aaa');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='abc' AND MaterialCode='bbb') INSERT INTO ProductMaterial VALUES ('abc','bbb');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='def' AND MaterialCode='bbb') INSERT INTO ProductMaterial VALUES ('def','bbb');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='def' AND MaterialCode='ccc') INSERT INTO ProductMaterial VALUES ('def','ccc');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='ghi' AND MaterialCode='aaa') INSERT INTO ProductMaterial VALUES ('ghi','aaa');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='ghi' AND MaterialCode='ccc') INSERT INTO ProductMaterial VALUES ('ghi','ccc');
IF NOT EXISTS (SELECT 1 FROM ProductMaterial WHERE ProductCode='ghi' AND MaterialCode='ddd') INSERT INTO ProductMaterial VALUES ('ghi','ddd');
IF NOT EXISTS (SELECT 1 FROM Production WHERE Email='teste@orvalis.example' AND Date='2026-10-20T10:30:00') INSERT INTO Production(Email,[Order],Date,Quantity,MaterialCode,CycleTime) VALUES ('teste@orvalis.example','111','2026-10-20T10:30:00',10,'aaa',35.5);
IF NOT EXISTS (SELECT 1 FROM Production WHERE Email='teste@orvalis.example' AND Date='2026-10-21T11:00:00') INSERT INTO Production(Email,[Order],Date,Quantity,MaterialCode,CycleTime) VALUES ('teste@orvalis.example','111','2026-10-21T11:00:00',5,'bbb',32.0);
IF NOT EXISTS (SELECT 1 FROM Production WHERE Email='maria@orvalis.example' AND Date='2026-10-22T09:15:00') INSERT INTO Production(Email,[Order],Date,Quantity,MaterialCode,CycleTime) VALUES ('maria@orvalis.example','222','2026-10-22T09:15:00',20,'ccc',50.0);
GO
