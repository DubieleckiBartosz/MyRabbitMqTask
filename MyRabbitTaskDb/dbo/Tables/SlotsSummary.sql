CREATE TABLE [dbo].[SlotsSummary] (
    [Id]                           INT            IDENTITY (1, 1) NOT NULL,
    [NumberMessages]               INT            NOT NULL,
    [FirstAndLastCharacters]       NVARCHAR (250) NOT NULL,
    [ChangedCharacters]            NVARCHAR (450) NULL,
    [MaxTimeBetweenContentChanges] NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

