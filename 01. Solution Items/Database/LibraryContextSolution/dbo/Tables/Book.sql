CREATE TABLE [dbo].[Book] (
    [Id]            CHAR (36)       NOT NULL,
    [Title]         NVARCHAR (50)   NOT NULL,
    [Description]   NVARCHAR (50)   NOT NULL,
    [Author]        NVARCHAR (50)   NOT NULL,
    [Quantity]      INT             NOT NULL, 
    [UpdatedAt]     DATETIME2 (7)       NULL,
    [CreatedAt]     DATETIME2 (7)   NOT NULL, 
    CONSTRAINT [PK_Book] PRIMARY KEY ([Id])
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'도서고유ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'도서 제목',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'Title'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'도서 설명',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'도서 작가',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'Author'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'수정일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'생성일시',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'CreatedAt'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'보유 수량',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Book',
    @level2type = N'COLUMN',
    @level2name = N'Quantity'