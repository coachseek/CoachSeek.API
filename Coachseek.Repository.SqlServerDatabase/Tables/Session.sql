CREATE TABLE [dbo].[Session] (
    [Id]               INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]       INT              NOT NULL,
    [Guid]             UNIQUEIDENTIFIER NOT NULL,
    [ParentId]         INT              NULL,
    [LocationId]       INT              NOT NULL,
    [CoachId]          INT              NOT NULL,
    [ServiceId]        INT              NOT NULL,
    [Name]             NVARCHAR (100)   NULL,
    [StartDate]        DATE             NOT NULL,
    [StartTime]        TIME (7)         NOT NULL,
    [Duration]         SMALLINT         NOT NULL,
    [StudentCapacity]  TINYINT          NOT NULL,
    [IsOnlineBookable] BIT              NOT NULL,
    [SessionCount]     TINYINT          NULL,
    [RepeatFrequency]  CHAR (1)         NULL,
    [SessionPrice]     DECIMAL (19, 4)  NULL,
    [CoursePrice]      DECIMAL (19, 4)  NULL,
    [Colour]           CHAR (12)        NOT NULL,
    CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Session_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id]),
    CONSTRAINT [FK_Session_Coach] FOREIGN KEY ([CoachId]) REFERENCES [dbo].[Coach] ([Id]),
    CONSTRAINT [FK_Session_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]),
    CONSTRAINT [FK_Session_Service] FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Service] ([Id]),
    CONSTRAINT [FK_Session_Session] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Session] ([Id])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Session_Guid]
    ON [dbo].[Session]([Guid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Session_BusinessId_Cover]
    ON [dbo].[Session]([BusinessId] ASC)
    INCLUDE([Id], [Guid], [ParentId], [LocationId], [CoachId], [ServiceId], [Name], [StartDate], [StartTime], [Duration], [StudentCapacity], [IsOnlineBookable], [SessionCount], [RepeatFrequency], [SessionPrice], [CoursePrice], [Colour]);

