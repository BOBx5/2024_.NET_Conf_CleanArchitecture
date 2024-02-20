CREATE TABLE [dbo].[User] (
    [Id]            CHAR (36)       NOT NULL,
    [Name]          NVARCHAR (50)   NOT NULL,
    [Status]        INT             NOT NULL,
    [Email]         NVARCHAR (50)   NOT NULL,
    [UpdatedAt]     DATETIME2 (7)       NULL,
    [CreatedAt]     DATETIME2 (7)   NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'수정일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'생성일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'CreatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'이메일',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Email'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'성명',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'유저고유ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'상태 (0: 가입대기, 1: 활동, 2: 정지)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'User',
    @level2type = N'COLUMN',
    @level2name = N'Status'