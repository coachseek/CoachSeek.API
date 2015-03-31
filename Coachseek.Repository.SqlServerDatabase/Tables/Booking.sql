CREATE TABLE [dbo].[Booking] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT              NOT NULL,
    [Guid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Booking_Guid] DEFAULT (newid()) NOT NULL,
    [CustomerId] INT              NOT NULL,
    [SessionId]  INT              NOT NULL,
    [ParentId] INT NULL, 
    CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Booking_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
    CONSTRAINT [FK_Booking_Session] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Session] ([Id]) 
);

