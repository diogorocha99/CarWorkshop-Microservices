--DROP TABLE PAYMENTS
DROP TABLE Payments;

--DROP TABLE 
DROP TABLE State;


--CREATE TABLE PAYMENTS
CREATE TABLE [dbo].[Payments](
	[PaymentId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[UserId] [int] NOT NULL,
	[RepairId] [uniqueidentifier] NOT NULL,
	[Plate] [varchar](20) NOT NULL,
	[StateId] [uniqueidentifier] NULL,
	[ServiceTypeId] [varchar](6) NOT NULL,
	[Price] [Float] NOT NULL,
	[PaymentConfirmedDate] [Datetime] NULL,

) ON [PRIMARY]

--CREATE TABLE STATE
CREATE TABLE [dbo].[State](
	[StateId] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[State] [varchar](20) NOT NULL,

) ON [PRIMARY]


INSERT INTO [dbo].Payments (PaymentId, UserId, RepairId, Plate, ServiceTypeId, Price) VALUES (NEWID(), 29053, '47baebfc-0307-4d98-abff-7cb7988d0fa4', '11-II-11', 'CHGOIL', 146)

INSERT INTO [dbo].State (StateId, State) VALUES (cast('be88317d-fcdc-4df2-9d2d-00d3886e46c0' AS uniqueidentifier), 'WAITPAY');
INSERT INTO [dbo].State (StateId, State) VALUES (cast('3dcb3c72-64cb-4057-b022-c91a09760bea' AS uniqueidentifier), 'DONEPAY');


-- FOREIGN KEY  STATE - PAYMENTS
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_StateId] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([StateId])
GO



