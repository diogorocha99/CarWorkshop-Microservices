-- DROP DATABASE
DROP DATABASE MSUsers;


-- CREATE DATABASE
CREATE DATABASE MSUsers;


-- CREATE USERS TABLE
CREATE TABLE [dbo].[Users](
	[UserId] [int] NOT NULL PRIMARY KEY,
	[Email] [nvarchar](500) NOT NULL UNIQUE,
	[Password] [nvarchar](150) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[AuthenticationAvailableTries] [int] NULL,
	[AuthenticationLastFail] [datetime] NULL,
	[Inactive] [bit] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
) ON [PRIMARY]


-- CREATE ROLES TABLE
CREATE TABLE [dbo].Roles (
	[RoleId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Role] [nvarchar](6) NOT NULL UNIQUE
	) ON [PRIMARY]


-- CREATE VEHICLES TYPE TABLE
CREATE TABLE [dbo].[VehiclesType](
	[VehicleTypeId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Description] [nvarchar](20) NOT NULL UNIQUE,
	[Type] [nvarchar](3) NOT NULL,
) ON [PRIMARY]


-- CREATE VEHICLES TABLE
CREATE TABLE [dbo].[Vehicles](
	[VehicleId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[UserId][int] NOT NULL,
	[LicensePlate] [nvarchar](20) NOT NULL UNIQUE,
	[VehicleTypeId] [uniqueidentifier] NOT NULL,
) ON [PRIMARY]





-- INSERTS USERS TABLE
INSERT INTO [dbo].Users (UserId, Email, Password, Name, Address, PhoneNumber, AuthenticationAvailableTries, AuthenticationLastFail, Inactive, CreationDate, RoleId) VALUES (29050, '', '', 'InternalAdmin', '', '', 0, NULL, cast(0 AS bit), '2008-10-29 14:56:59', cast('a739789d-6a7f-45d4-bd30-394dbc223f57' AS uniqueidentifier));
INSERT INTO [dbo].Users (UserId, Email, Password, Name, Address, PhoneNumber, AuthenticationAvailableTries, AuthenticationLastFail, Inactive, CreationDate, RoleId) VALUES (29051, 'owner@email.pt', 'PeLJ1KamIeEOHV7n8D0ioA==', 'OwnerName', 'OwnerAddress', 'OwnerNumber', 0, NULL, cast(0 AS bit), '2008-10-29 14:56:59', cast('a739789d-6a7f-45d4-bd30-394dbc223f57' AS uniqueidentifier));
INSERT INTO [dbo].Users (UserId, Email, Password, Name, Address, PhoneNumber, AuthenticationAvailableTries, AuthenticationLastFail, Inactive, CreationDate, RoleId) VALUES (29052, 'manager@email.pt', 'IgqQfU8BaAhJNifymEBYsA==', 'ManagerName', 'ManagerAddress', 'ManagerNumber', 0, NULL, cast(0 AS bit), GETDATE(), cast('db32fd4d-9724-4e5b-ba0b-50cbcbca120a' AS uniqueidentifier));
INSERT INTO [dbo].Users (UserId, Email, Password, Name, Address, PhoneNumber, AuthenticationAvailableTries, AuthenticationLastFail, Inactive, CreationDate, RoleId) VALUES (29053, 'user@email.pt', 'C2FfpK4j1QRY2yB4I8TJNQ==', 'UserName', 'UserAddress', 'UserNumber', 0, NULL, cast(0 AS bit), '2008-10-29 14:56:59', cast('b6149a90-7994-44c1-b832-113cda923ee6' AS uniqueidentifier));


-- INSERT VEHICLES TABLE
INSERT INTO [dbo].Vehicles (VehicleId, UserId, LicensePlate, VehicleTypeId) VALUES (cast('57fcf45f-84ca-4f52-8f43-d1872de7b294' AS uniqueidentifier), 29052, '34-SD-43', cast('8ab3098c-b5ff-4570-9d32-08deb2c5ea73' AS uniqueidentifier));
INSERT INTO [dbo].Vehicles (VehicleId, UserId, LicensePlate, VehicleTypeId) VALUES (cast('8062e193-5d41-46b9-b42d-2bf114e76787' AS uniqueidentifier), 29053, '87-34-AD', cast('8ab3098c-b5ff-4570-9d32-08deb2c5ea73' AS uniqueidentifier));
INSERT INTO [dbo].Vehicles (VehicleId, UserId, LicensePlate, VehicleTypeId) VALUES (cast('089ef419-5d6b-4ff2-a7dc-d0e6b76fb22c' AS uniqueidentifier), 29053, 'AA-DD-43', cast('fbfc63a8-a66c-4249-80f9-b3bc72238f5e' AS uniqueidentifier));


-- INSERTS ROLES TABLE
INSERT INTO [dbo].Roles (RoleId, Role) VALUES (cast('a739789d-6a7f-45d4-bd30-394dbc223f57' AS uniqueidentifier), 'ADMIN');
INSERT INTO [dbo].Roles (RoleId, Role) VALUES (cast('db32fd4d-9724-4e5b-ba0b-50cbcbca120a' AS uniqueidentifier), 'MANGR');
INSERT INTO [dbo].Roles (RoleId, Role) VALUES (cast('b6149a90-7994-44c1-b832-113cda923ee6' AS uniqueidentifier), 'USERS');


-- INSERT VEHICLETYPE TABLE
INSERT INTO [dbo].VehiclesType (VehicleTypeId, Description, Type) VALUES (cast('8ab3098c-b5ff-4570-9d32-08deb2c5ea73' AS uniqueidentifier), 'CAR', 'CAR');
INSERT INTO [dbo].VehiclesType (VehicleTypeId, Description, Type) VALUES (cast('8090855e-9318-4720-9ce0-931e7b4620e4' AS uniqueidentifier), 'MOTORBIKE', 'MBK');
INSERT INTO [dbo].VehiclesType (VehicleTypeId, Description, Type) VALUES (cast('fbfc63a8-a66c-4249-80f9-b3bc72238f5e' AS uniqueidentifier), 'TRUCK', 'TRK');





-- FOREIGN KEY VEHICLE - VEHICLETYPES
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicle_VehiclesType] FOREIGN KEY([VehicleTypeId])
REFERENCES [dbo].[VehiclesType] ([VehicleTypeId])
GO


-- FOREIGN KEY VEHICLES - USERS
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD CONSTRAINT [FK_Vehicles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO


-- FOREIGN KEY USERS - ROLES
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
