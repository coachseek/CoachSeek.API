CREATE TABLE [dbo].[Business] (
    [Id]                        INT              IDENTITY (1, 1) NOT NULL,
    [Guid]                      UNIQUEIDENTIFIER NOT NULL,
    [Name]                      NVARCHAR (100)   NOT NULL,
    [Domain]                    NVARCHAR (100)   NOT NULL,
    [Currency]                  NCHAR (3)        NOT NULL,
    [PaymentProvider]           NVARCHAR (50)    NULL,
    [MerchantAccountIdentifier] NVARCHAR (100)   NULL,
    [CreatedOn]                 DATETIME2 (7)    NOT NULL,
    [IsOnlinePaymentEnabled]    BIT              NOT NULL,
    [ForceOnlinePayment]        BIT              NOT NULL,
    [IsTesting]                 BIT              NOT NULL,
    [Sport]                     NVARCHAR (100)   NULL,
    [AuthorisedUntil]           DATETIME2 (7)    NULL,
    [Subscription]              NVARCHAR (50)    NULL,
    [UseProRataPricing]         BIT              NOT NULL,
    CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED ([Id] ASC)
);



















GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Business_Guid]
    ON [dbo].[Business]([Guid] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Business_Domain]
    ON [dbo].[Business]([Domain] ASC);

