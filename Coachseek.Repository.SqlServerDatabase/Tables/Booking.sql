CREATE TABLE [dbo].[Booking] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]      INT              NOT NULL,
    [Guid]            UNIQUEIDENTIFIER NOT NULL,
    [CustomerId]      INT              NOT NULL,
    [SessionId]       INT              NOT NULL,
    [ParentId]        INT              NULL,
    [PaymentStatus]   NVARCHAR (50)    NULL,
    [HasAttended]     BIT              NULL,
    [IsOnlineBooking] BIT              NULL,
    [CreatedOn]       DATETIME2 (7)    CONSTRAINT [DF_Booking_CreatedOn] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Booking_Booking] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Booking] ([Id]),
    CONSTRAINT [FK_Booking_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id]),
    CONSTRAINT [FK_Booking_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
    CONSTRAINT [FK_Booking_Session] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Session] ([Id])
);














GO
CREATE NONCLUSTERED INDEX [IX_Booking_BusinessId_Cover]
    ON [dbo].[Booking]([BusinessId] ASC)
    INCLUDE([Id], [Guid], [CustomerId], [SessionId], [ParentId], [PaymentStatus], [HasAttended]);

