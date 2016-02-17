CREATE TABLE [dbo].[DiscountCode] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]      INT              NOT NULL,
    [Guid]            UNIQUEIDENTIFIER NOT NULL,
    [Code]            NCHAR (10)       NOT NULL,
    [DiscountPercent] INT              NOT NULL,
    [IsActive]        BIT              NOT NULL,
    CONSTRAINT [PK_DiscountCode] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DiscountCode_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_BusinessId_Code]
    ON [dbo].[DiscountCode]([BusinessId] ASC, [Code] ASC);

