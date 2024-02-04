
-- DROP TABLE GARAGE
DROP TABLE Garage

--DROP TABLE GARAGE_USER
DROP TABLE Garage_User


-- CREATE TABLE GARAGE
CREATE TABLE [dbo].[Garage](
	[GarageId] [int] NOT NULL PRIMARY KEY,
	[Address] VARCHAR(150) NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	[Postal_Code] VARCHAR(10) NOT NULL,
) ON [PRIMARY]

--CREATE TABLE GARAGE_USER
CREATE TABLE [dbo].Garage_User (
	[GarageId] [int] NOT NULL,
	[UserId] [int] NOT NULL,

)


--FOREIGN KEY USER


-- FOREIGN KEY REPAIR - REPAIRSPARTS
ALTER TABLE [dbo].[Garage_User] WITH CHECK ADD CONSTRAINT [FK_Garage] FOREIGN KEY([GarageId])
REFERENCES [dbo].[Garage] ([GarageId])
GO




--INSERTS GARAGE
INSERT INTO [dbo].Garage (GarageId, Address, Name, Postal_Code) VALUES (1, '1-7-14 Saidaiji Akodacho', 'Majuro', '0282-403');
INSERT INTO [dbo].Garage (GarageId, Address, Name, Postal_Code) VALUES (2, '197 Abingdon Rd, Cumnor', 'Jakarta', '0226-362');
INSERT INTO [dbo].Garage (GarageId, Address, Name, Postal_Code) VALUES (3, '739 Riverview Road', 'Malé', '8395-743');


--INSERT GARAGE_USER