CREATE TABLE [dbo].[Business] (
    [Id]                        INT              IDENTITY (1, 1) NOT NULL,
    [Guid]                      UNIQUEIDENTIFIER CONSTRAINT [DF_Business_Guid] DEFAULT (newid()) NOT NULL,
    [Name]                      NVARCHAR (100)   NOT NULL,
    [Domain]                    NVARCHAR (100)   CONSTRAINT [DF_Business_Domain] DEFAULT ('') NOT NULL,
    [Currency]                  NCHAR (3)        NOT NULL,
    [PaymentProvider]           NVARCHAR (50)    NULL,
    [MerchantAccountIdentifier] NVARCHAR (100)   NULL,
    [CreatedOn]                 DATETIME2 (7)    CONSTRAINT [DF_Business_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [IsOnlinePaymentEnabled]    BIT              CONSTRAINT [DF_Business_IsOnlinePaymentEnabled] DEFAULT ((0)) NOT NULL,
    [ForceOnlinePayment]        BIT              CONSTRAINT [DF_Business_ForceOnlinePayment] DEFAULT ((0)) NOT NULL,
    [IsTesting]                 BIT              CONSTRAINT [DF_Business_IsTesting] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED ([Id] ASC)
);



GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Business_Guid]
    ON [dbo].[Business]([Guid] ASC);

