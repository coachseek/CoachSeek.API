CREATE TABLE [dbo].[Service] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]       INT              NOT NULL,
    [Guid]             UNIQUEIDENTIFIER CONSTRAINT [DF_Service_Guid] DEFAULT (newid()) NOT NULL,
    [Name]             NVARCHAR (100)   NOT NULL,
    [Description]      NVARCHAR (500)   NULL,
    [Duration]         SMALLINT         NULL,
    [StudentCapacity]  TINYINT          NULL,
    [IsOnlineBookable] BIT              NULL,
    [SessionCount]     TINYINT          CONSTRAINT [DF_Service_SessionCount] DEFAULT ((1)) NOT NULL,
    [RepeatFrequency]  CHAR (1)         NULL,
    [SessionPrice]     DECIMAL (19, 4)  NULL,
    [CoursePrice]      DECIMAL (19, 4)  NULL,
    [Colour]           CHAR (12)        CONSTRAINT [DF_Service_Colour] DEFAULT ('Green') NOT NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Service_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Service_Guid]
    ON [dbo].[Service]([Guid] ASC);

