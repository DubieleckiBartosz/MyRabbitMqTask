CREATE TABLE [dbo].[Messages] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Content]   NVARCHAR (20)  NOT NULL,
    [ElementId] NVARCHAR (150) NOT NULL,
    [Type]      NVARCHAR (50)  NOT NULL,
    [Date]      DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

