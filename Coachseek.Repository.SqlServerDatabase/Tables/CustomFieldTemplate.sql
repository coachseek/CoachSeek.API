CREATE TABLE [dbo].[CustomFieldTemplate] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId] INT              NOT NULL,
    [Guid]       UNIQUEIDENTIFIER CONSTRAINT [DF_CustomFieldTemplate_Guid] DEFAULT (newid()) NOT NULL,
    [Type]       NVARCHAR (50)    NOT NULL,
    [Key]        NVARCHAR (50)    NOT NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    [IsRequired] BIT              NOT NULL,
    CONSTRAINT [PK_CustomFieldTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CustomFieldTemplate_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);

