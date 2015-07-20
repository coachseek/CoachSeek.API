CREATE TABLE [dbo].[Transaction] (
    [Id]              NVARCHAR (50)    NOT NULL,
    [PaymentProvider] NVARCHAR (50)    NOT NULL,
    [Type]            NVARCHAR (50)    NOT NULL,
    [Status]          NVARCHAR (50)    NOT NULL,
    [TransactionDate] DATETIME2 (7)    NOT NULL,
    [ProcessedDate]   DATETIME2 (7)    NOT NULL,
    [PayerFirstName]  NVARCHAR (50)    NOT NULL,
    [PayerLastName]   NVARCHAR (50)    NOT NULL,
    [PayerEmail]      NVARCHAR (100)   NOT NULL,
    [MerchantId]      UNIQUEIDENTIFIER NOT NULL,
    [MerchantName]    NVARCHAR (100)   NOT NULL,
    [MerchantEmail]   NVARCHAR (100)   NOT NULL,
    [ItemId]          UNIQUEIDENTIFIER NOT NULL,
    [ItemName]        NVARCHAR (200)   NOT NULL,
    [ItemCurrency]    NCHAR (3)        NOT NULL,
    [ItemAmount]      DECIMAL (19, 4)  NOT NULL,
    [OriginalMessage] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC, [PaymentProvider] ASC)
);





