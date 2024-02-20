CREATE TABLE [dbo].[OutboxMessage]
(
    [Id]            CHAR (36)       NOT NULL,
    [Type]          NVARCHAR(100)   NOT NULL, 
    [Content]       NVARCHAR(MAX)   NOT NULL, 
    [OccurredOn]    DATETIME2       NOT NULL, 
    [ProcessedOn]   DATETIME2           NULL, 
    [Error]         NVARCHAR(MAX)       NULL, 
    CONSTRAINT [PK_OutboxMessage] PRIMARY KEY ([Id])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'메시지ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'유형',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = N'Type'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'내용',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = N'Content'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'발생일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = 'OccurredOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'처리일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = N'ProcessedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'에러내용',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OutboxMessage',
    @level2type = N'COLUMN',
    @level2name = N'Error'