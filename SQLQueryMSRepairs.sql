-- DROP DATABASE
DROP DATABASE MSRepairs;


-- CREATE DATABASE
CREATE DATABASE MSRepairs;


-- CREATE REPAIRS TABLE
CREATE TABLE [dbo].[Repairs] (
	[RepairId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[RequestId] [uniqueidentifier] NOT NULL,
	[ManagerUserId] [int] NOT NULL,
	[GarageId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[StateId] VARCHAR(6) NOT NULL,
	[ServiceTypeId] VARCHAR(6) NULL,
	[RepairCreatedDateTime] [datetime] NOT NULL,
	[RepairDoneDatime] [datetime] NULL,
	[WorkTime][int] NULL,
	[Notes] VARCHAR(50) NULL,
) ON [PRIMARY]


-- CREATE PARTSREPAIRS TABLE
CREATE TABLE [dbo].[RepairsParts](
	[RepairId] [uniqueidentifier] NOT NULL,
	[PartsId] [uniqueidentifier] NOT NULL,
) ON [PRIMARY]


-- CREATE PARTS TABLE
CREATE TABLE [dbo].[Parts](
	[PartsId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[StockId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [float] NOT NULL,
) ON [PRIMARY]


-- CREATE STOCK TABLE
CREATE TABLE [dbo].[Stock](
	[StockId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[GarageId] [int] NOT NULL,
	[PartName] VARCHAR(150) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [float] NOT NULL,
) ON [PRIMARY]


-- CREATE STATE TABLE
CREATE TABLE [dbo].[State](
	[StateId] VARCHAR(6) NOT NULL PRIMARY KEY,
	[StateType] VARCHAR(100) NOT NULL,
) ON [PRIMARY]


-- CREATE SERVICETYPE TABLE
CREATE TABLE [dbo].[ServiceType](
	[ServiceTypeId] VARCHAR(6) NOT NULL PRIMARY KEY,
	[ServiceTypeDescription] VARCHAR(100) NOT NULL,
) ON [PRIMARY]





-- INSERTS REPAIRS TABLE
INSERT INTO [dbo].Repairs (RepairId, RequestId, ManagerUserId, GarageId, StateId, ServiceTypeId, RepairCreatedDateTime, RepairDoneDatime, WorkTime, Notes) VALUES (cast('fd9f6839-4c2d-403f-b0a4-8e3e648580b0' AS uniqueidentifier), cast('eb51db56-c0c4-4a2f-85b8-3d0fcf0dca6e' AS uniqueidentifier), 29052, 1, 'NTCMPL', 'CHGOIL', GETDATE(), NULL, NULL, NULL);

UPDATE [dbo].Repairs SET RepairDoneDatime = GETDATE()
UPDATE [dbo].Repairs SET WorkTime = 3 WHERE RepairId = 'fd9f6839-4c2d-403f-b0a4-8e3e648580b0'
UPDATE [dbo].Repairs SET Notes = 'Limpei o Carro, Quero um Aumento'

-- INSERTS REPAIRSPARTS TABLE
INSERT INTO [dbo].RepairsParts (RepairId, PartsId) VALUES (cast('fd9f6839-4c2d-403f-b0a4-8e3e648580b0' AS uniqueidentifier), cast('33602a2e-e06a-4e56-8c5e-7773797aa0fb' AS uniqueidentifier));
INSERT INTO [dbo].RepairsParts (RepairId, PartsId) VALUES (cast('fd9f6839-4c2d-403f-b0a4-8e3e648580b0' AS uniqueidentifier), cast('04f7c956-cfe0-4a08-8d09-feb671bdd2bd' AS uniqueidentifier));


-- INSERTS PARTS TABLE
INSERT INTO [dbo].Parts (PartsId, StockId, Quantity, Price) VALUES (cast('33602a2e-e06a-4e56-8c5e-7773797aa0fb' AS uniqueidentifier), cast('ae06d769-d48a-4300-a6db-d54f65f1f99f' AS uniqueidentifier), 1, 60);
INSERT INTO [dbo].Parts (PartsId, StockId, Quantity, Price) VALUES (cast('04f7c956-cfe0-4a08-8d09-feb671bdd2bd' AS uniqueidentifier), cast('b77ea7dd-60f4-421d-9e03-8dc0e2edf507' AS uniqueidentifier), 1, 15);


-- INSERTS STOCK TABLE
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('ae06d769-d48a-4300-a6db-d54f65f1f99f' AS uniqueidentifier), 1,'Oleo 10W 5', 5, 60);
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('91cec96d-341f-40b9-9567-b17e0947299b' AS uniqueidentifier), 1,'Correia Distribuição', 8, 20);
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('e6d4c7fc-c925-48f4-9eb9-bfbe3b3237d0' AS uniqueidentifier), 1,'Pneu 255 65 16', 12, 40);
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('b77ea7dd-60f4-421d-9e03-8dc0e2edf507' AS uniqueidentifier), 1,'Filtro de Oleo', 6, 15);
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('b60c7cf2-abd4-4005-905b-6e4902fd5761' AS uniqueidentifier), 1,'Recarga Ar Condicionado', 2, 35);
INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('d38a1562-12cf-4811-85d1-46b96b9fac11' AS uniqueidentifier), 1,'Filtro de Ar', 14, 25);

INSERT INTO [dbo].Stock (StockId, GarageId, PartName, Quantity, Price) VALUES (cast('ae06d769-d48a-4300-a6db-d54f65f1f99f' AS uniqueidentifier), 2,'Oleo 10W 5', 5, 60);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('91cec96d-341f-40b9-9567-b17e0947299b' AS uniqueidentifier), 2,'Correia Distribuição', 8, 20);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('e6d4c7fc-c925-48f4-9eb9-bfbe3b3237d0' AS uniqueidentifier), 2,'Pneu 255 65 16', 12, 40);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('b77ea7dd-60f4-421d-9e03-8dc0e2edf507' AS uniqueidentifier), 2,'Filtro de Oleo', 6, 15);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('b60c7cf2-abd4-4005-905b-6e4902fd5761' AS uniqueidentifier), 2,'Recarga Ar Condicionado', 2, 35);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('d38a1562-12cf-4811-85d1-46b96b9fac11' AS uniqueidentifier), 2,'Filtro de Ar', 14, 25);

INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('ae06d769-d48a-4300-a6db-d54f65f1f99f' AS uniqueidentifier), 3,'Oleo 10W 5', 5, 60);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('91cec96d-341f-40b9-9567-b17e0947299b' AS uniqueidentifier), 3,'Correia Distribuição', 8, 20);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('e6d4c7fc-c925-48f4-9eb9-bfbe3b3237d0' AS uniqueidentifier), 3,'Pneu 255 65 16', 12, 40);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('b77ea7dd-60f4-421d-9e03-8dc0e2edf507' AS uniqueidentifier), 3,'Filtro de Oleo', 6, 15);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('b60c7cf2-abd4-4005-905b-6e4902fd5761' AS uniqueidentifier), 3,'Recarga Ar Condicionado', 2, 35);
INSERT INTO [dbo].Stock (StockId, GarageId,PartName, Quantity, Price) VALUES (cast('d38a1562-12cf-4811-85d1-46b96b9fac11' AS uniqueidentifier), 3,'Filtro de Ar', 14, 25);


-- INSERTS STATE TABLE
INSERT INTO [dbo].State (StateId, StateType) VALUES ('NTCMPL', 'Not Completed'); 
INSERT INTO [dbo].State (StateId, StateType) VALUES ('CMPLET', 'Completed'); 
INSERT INTO [dbo].State (StateId, StateType) VALUES ('INPROG', 'In Progess'); 


-- INSERTS SERVICETYPE TABLE
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('CHGOIL', 'Change Oil'); 
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('CHGDIS', 'Change Distribution'); 
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('REVSON', 'Vehicle Revision'); 
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('CHGTRS', 'Change Tires'); 
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('GERMEC', 'General Mechanic'); 
INSERT INTO [dbo].ServiceType (ServiceTypeId, ServiceTypeDescription) VALUES ('OTHERS', 'Others'); 





-- FOREIGN KEY PARTSREPAIR - STOCK
ALTER TABLE [dbo].[Parts]  WITH CHECK ADD  CONSTRAINT [FK_StockId] FOREIGN KEY([StockId])
REFERENCES [dbo].[Stock] ([StockId])
GO


-- FOREIGN KEY REPAIR - REPAIRSPARTS
ALTER TABLE [dbo].[RepairsParts] WITH CHECK ADD CONSTRAINT [FK_RepairsParts] FOREIGN KEY([RepairId])
REFERENCES [dbo].[Repairs] ([RepairId])
GO


-- FOREIGN KEY PARTS - REPAIRSPARTS
ALTER TABLE [dbo].[RepairsParts]  WITH CHECK ADD CONSTRAINT [FK_PartsRepairs] FOREIGN KEY([PartsId])
REFERENCES [dbo].[Parts] ([PartsId])
GO


-- FOREIGN KEY REPAIR - STATE
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD CONSTRAINT [FK_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([StateId])
GO


-- FOREIGN KEY REPAIR - SERVICETYPE
ALTER TABLE [dbo].[Repairs]  WITH CHECK ADD CONSTRAINT [FK_ServiceType] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[ServiceType] ([ServiceTypeId])
GO





SELECT s.PartName FROM [dbo].Repairs r
		INNER JOIN [dbo].RepairsParts rp ON r.RepairId = rp.RepairId
		INNER JOIN [dbo].Parts p ON rp.PartsId = p.PartsId
		INNER JOIN [dbo].Stock s ON p.StockId = s.StockId
WHERE rp.RepairId = 'fd9f6839-4c2d-403f-b0a4-8e3e648580b0'



SELECT SUM(p.Quantity * s.Price + (p.Quantity * s.Price * 0.23)) Price
FROM [dbo].Repairs r
		INNER JOIN [dbo].RepairsParts rp ON r.RepairId = rp.RepairId
		INNER JOIN [dbo].Parts p ON rp.PartsId = p.PartsId
		INNER JOIN [dbo].Stock s ON p.StockId = s.StockId
WHERE rp.RepairId = 'fd9f6839-4c2d-403f-b0a4-8e3e648580b0'


SELECT s.PartName, SUM(s.Price) FROM [dbo].Parts p
	INNER JOIN [dbo].Stock s ON p.StockId = s.StockId
	INNER JOIN [dbo].RepairsParts rp ON p.PartsId = rp.PartsId
	INNER JOIN [dbo].Repairs r ON rp.RepairId = r.RepairId
WHERE r.RepairId = 'fd9f6839-4c2d-403f-b0a4-8e3e648580b0'
GROUP BY s.PartName


SELECT r.RepairId, r.RequestId, r.ManagerUserId, r.ServiceTypeId, r.WorkTime, r.Notes, 
(SELECT SUM(stk.Price) FROM [dbo].Stock stk
INNER JOIN [dbo].Parts pts ON stk.StockId = pts.StockId
INNER JOIN [dbo].RepairsParts rprpts ON pts.PartsId = rprpts.PartsId
INNER JOIN [dbo].Repairs rpr ON rprpts.RepairId = rpr.RepairId)
FROM [dbo].Repairs r
INNER JOIN [dbo].RepairsParts rp ON r.RepairId = rp.RepairId
INNER JOIN [dbo].Parts p ON rp.PartsId = p.PartsId
INNER JOIN [dbo].Stock s ON p.StockId = s.StockId
WHERE r.StateId = 'INPROG' OR r.StateId = 'NTCMPL'
GROUP BY r.RepairId


SELECT SUM(s.Price) FROM [dbo].Parts p
	INNER JOIN [dbo].Stock s ON p.StockId = s.StockId
	INNER JOIN [dbo].RepairsParts rp ON p.PartsId = rp.PartsId
	INNER JOIN [dbo].Repairs r ON rp.RepairId = r.RepairId
GROUP BY r.RequestId


SELECT PartName, (SELECT SUM(Price) FROM [dbo].Stock) 
FROM [dbo].Stock
ORDER BY Price DESC


SELECT * FROM [dbo].Repairs WHERE StateId = 'INPROG' OR StateId = 'NTCMPL'




UPDATE [dbo].Repairs SET  StateId = 'INPROG', ServiceTypeId = 'CHGOIL', WorkTime =  (SELECT MAX(WorkTime) from [dbo].Repairs r WHERE RepairId = 'fd6fb70f-a4a4-447e-803e-31edf1db2504') + 1, Notes = ''  WHERE RepairId = 'fd6fb70f-a4a4-447e-803e-31edf1db2504';

SELECT * FROM [dbo].Stock

UPDATE [dbo].Stock SET Quantity = ((SELECT MAX(Quantity) from [dbo].Stock r WHERE StockId = 'D38A1562-12CF-4811-85D1-46B96B9FAC11') - 1) WHERE StockId = 'D38A1562-12CF-4811-85D1-46B96B9FAC11';





DECLARE @newPartsId uniqueidentifier; SET @newPartsId = NEWID(); INSERT INTO [dbo].Parts (PartsId, StockId, Quantity, Price) VALUES (@newPartsId, cast('D38A1562-12CF-4811-85D1-46B96B9FAC11' AS uniqueidentifier), 1, (SELECT Price FROM [dbo].Stock WHERE StockId = 'D38A1562-12CF-4811-85D1-46B96B9FAC11') * 1); INSERT INTO [dbo].RepairsParts (RepairId, PartsId) VALUES (cast('fd6fb70f-a4a4-447e-803e-31edf1db2504' AS uniqueidentifier), @newPartsId);



