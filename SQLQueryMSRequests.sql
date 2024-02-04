DROP DATABASE MSRequests

-- CREATE DATABASE Requests
CREATE DATABASE MSRequests;


-- CREATE REQUEST VALIDATION TABLE
CREATE TABLE [dbo].[Requests](
	[RequestId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[ManagerUserId] [int] NULL,
	[UserId] [int] NOT NULL,
	[GarageId] [int] NOT NULL,
	[LicensePlate] [nvarchar](20) NOT NULL, 
	[Validated] [bit] NULL,
	[RequestDate] [datetime] NOT NULL,
	[ValidateDateTime] [datetime] NULL,
	[Inactive] [bit] NOT NULL,
) ON [PRIMARY]


-- INSERT REQUEST VALIDATION TABLE
INSERT INTO [dbo].Requests (RequestId, ManagerUserId, UserId, LicensePlate, Validated, RequestDate, ValidateDateTime, Inactive) VALUES (cast('eb51db56-c0c4-4a2f-85b8-3d0fcf0dca6e' AS uniqueidentifier), NULL, 29448, '34-SD-43' , NULL, GETDATE(), NULL, cast(0 AS bit));

--UPDATE REQUEST VALIDATION TABLE
UPDATE dbo.[Requests] SET ManagerUserId = 29449, Validated = 1, ValidateDateTime = GETDATE(), Inactive = cast(0 AS bit) WHERE RequestId = '20dc520f-17da-46f4-bade-e466a9724808';
