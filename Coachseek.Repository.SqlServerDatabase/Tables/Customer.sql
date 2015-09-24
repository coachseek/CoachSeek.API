CREATE TABLE [dbo].[Customer] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT              NOT NULL,
    [Guid]       UNIQUEIDENTIFIER NOT NULL,
    [FirstName]  NVARCHAR (50)    NOT NULL,
    [LastName]   NVARCHAR (50)    NOT NULL,
    [Email]      NVARCHAR (100)   NULL,
    [Phone]      NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Customer_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);












GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Customer_Guid]
    ON [dbo].[Customer]([Guid] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Customer_BusinessId_Cover]
    ON [dbo].[Customer]([BusinessId] ASC, [LastName] ASC, [FirstName] ASC, [Email] ASC)
    INCLUDE([Id], [Guid], [Phone]);

