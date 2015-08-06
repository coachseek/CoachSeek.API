CREATE TABLE [dbo].[EmailTemplate] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT            NOT NULL,
    [Type]       NVARCHAR (50)  NOT NULL,
    [Subject]    NVARCHAR (200) NOT NULL,
    [Body]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EmailTemplate_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_EmailTemplate_BusinessId_Type]
    ON [dbo].[EmailTemplate]([BusinessId] ASC, [Type] ASC);

