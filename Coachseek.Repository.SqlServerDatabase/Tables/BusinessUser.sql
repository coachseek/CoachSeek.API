CREATE TABLE [dbo].[BusinessUser] (
    [Id]   INT              IDENTITY (1, 1) NOT NULL,
    [Guid] UNIQUEIDENTIFIER CONSTRAINT [DF_BusinessUser_Guid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_BusinessUser] PRIMARY KEY CLUSTERED ([Id] ASC)
);

