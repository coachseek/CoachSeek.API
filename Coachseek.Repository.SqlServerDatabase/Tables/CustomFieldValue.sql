CREATE TABLE [dbo].[CustomFieldValue] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT            NOT NULL,
    [Type]       NVARCHAR (50)  NOT NULL,
    [TypeId]     INT            NOT NULL,
    [Key]        NVARCHAR (50)  NOT NULL,
    [Value]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CustomFieldValue] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CustomFieldValue_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);

