CREATE TABLE [dbo].[Coach] (
    [Id]                   INT              IDENTITY (1, 1) NOT NULL,
    [BusinessId]           INT              NOT NULL,
    [Guid]                 UNIQUEIDENTIFIER NOT NULL,
    [FirstName]            NVARCHAR (50)    NOT NULL,
    [LastName]             NVARCHAR (50)    NOT NULL,
    [Email]                NVARCHAR (100)   NOT NULL,
    [Phone]                NVARCHAR (50)    NOT NULL,
    [MondayIsAvailable]    BIT              CONSTRAINT [DF_Coach_MondayIsAvailable] DEFAULT ((0)) NOT NULL,
    [MondayStartTime]      NCHAR (5)        NULL,
    [MondayFinishTime]     NCHAR (5)        NULL,
    [TuesdayIsAvailable]   BIT              CONSTRAINT [DF_Coach_TuesdayIsAvailable] DEFAULT ((0)) NOT NULL,
    [TuesdayStartTime]     NCHAR (5)        NULL,
    [TuesdayFinishTime]    NCHAR (5)        NULL,
    [WednesdayIsAvailable] BIT              CONSTRAINT [DF_Coach_WednesdayIsAvailable] DEFAULT ((0)) NOT NULL,
    [WednesdayStartTime]   NCHAR (5)        NULL,
    [WednesdayFinishTime]  NCHAR (5)        NULL,
    [ThursdayIsAvailable]  BIT              CONSTRAINT [DF_Coach_ThursdayIsAvailable] DEFAULT ((0)) NOT NULL,
    [ThursdayStartTime]    NCHAR (5)        NULL,
    [ThursdayFinishTime]   NCHAR (5)        NULL,
    [FridayIsAvailable]    BIT              CONSTRAINT [DF_Coach_FridayIsAvailable] DEFAULT ((0)) NOT NULL,
    [FridayStartTime]      NCHAR (5)        NULL,
    [FridayFinishTime]     NCHAR (5)        NULL,
    [SaturdayIsAvailable]  BIT              CONSTRAINT [DF_Coach_SaturdayIsAvailable] DEFAULT ((0)) NOT NULL,
    [SaturdayStartTime]    NCHAR (5)        NULL,
    [SaturdayFinishTime]   NCHAR (5)        NULL,
    [SundayIsAvailable]    BIT              CONSTRAINT [DF_Coach_SundayIsAvailable] DEFAULT ((0)) NOT NULL,
    [SundayStartTime]      NCHAR (5)        NULL,
    [SundayFinishTime]     NCHAR (5)        NULL,
    CONSTRAINT [PK_Coach] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Coach_Business] FOREIGN KEY ([BusinessId]) REFERENCES [dbo].[Business] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Coach_Guid]
    ON [dbo].[Coach]([Guid] ASC);

