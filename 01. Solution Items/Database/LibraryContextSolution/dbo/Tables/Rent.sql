CREATE TABLE [dbo].[Rent] (
    [Id]            CHAR (36)       NOT NULL,
    [BookId]        CHAR (36)       NOT NULL,
    [UserId]        CHAR (36)       NOT NULL,
    [DueDate]       DATETIME2 (7)   NOT NULL,
    [BorrowedAt]    DATETIME2 (7)   NOT NULL,
    [ReturnedAt]    DATETIME2 (7)       NULL,
    [UpdatedAt]     DATETIME2 (7)       NULL,
    [CreatedAt]     DATETIME2 (7)   NOT NULL, 
    CONSTRAINT [PK_Rent] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Rent_Book] FOREIGN KEY ([BookId]) REFERENCES [Book]([Id]),
    CONSTRAINT [FK_Rent_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
);


GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'수정일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'생성일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'CreatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'대여고유ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'대여도서ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'BookId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'대여자ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'UserId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'반납기한',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'DueDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'대여일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'BorrowedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'반납일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Rent',
    @level2type = N'COLUMN',
    @level2name = N'ReturnedAt'