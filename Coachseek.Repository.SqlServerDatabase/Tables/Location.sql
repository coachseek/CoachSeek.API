CREATE TABLE [dbo].[Location] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT              NOT NULL,
    [Guid]       UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Location_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Location_Guid]
    ON [dbo].[Location]([Guid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Location_BusinessId_Cover]
    ON [dbo].[Location]([BusinessId] ASC)
    INCLUDE([Id], [Guid], [Name]);

