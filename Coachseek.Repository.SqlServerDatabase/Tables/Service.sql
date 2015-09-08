CREATE TABLE [dbo].[Service] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]       INT              NOT NULL,
    [Guid]             UNIQUEIDENTIFIER NOT NULL,
    [Name]             NVARCHAR (100)   NOT NULL,
    [Description]      NVARCHAR (500)   NULL,
    [Duration]         SMALLINT         NULL,
    [StudentCapacity]  TINYINT          NULL,
    [IsOnlineBookable] BIT              NULL,
    [SessionCount]     TINYINT          NOT NULL,
    [RepeatFrequency]  CHAR (1)         NULL,
    [SessionPrice]     DECIMAL (19, 4)  NULL,
    [CoursePrice]      DECIMAL (19, 4)  NULL,
    [Colour]           CHAR (12)        NOT NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Service_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Service_Guid]
    ON [dbo].[Service]([Guid] ASC);

