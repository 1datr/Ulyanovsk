
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/19/2014 17:37:10
-- Generated from EDMX file: D:\программы\StackerWPF\WpfStackerLibrary\ModelStacker.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [cellcontent];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ProductCellContent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CellContents] DROP CONSTRAINT [FK_ProductCellContent];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[CellContents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CellContents];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CellContents'
CREATE TABLE [dbo].[CellContents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StackerID] int  NOT NULL,
    [CellID] int  NOT NULL,
    [Count] int  NOT NULL,
    [ChangeDate] datetime  NOT NULL,
    [Product_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CellContents'
ALTER TABLE [dbo].[CellContents]
ADD CONSTRAINT [PK_CellContents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Product_Id] in table 'CellContents'
ALTER TABLE [dbo].[CellContents]
ADD CONSTRAINT [FK_ProductCellContent]
    FOREIGN KEY ([Product_Id])
    REFERENCES [dbo].[Products]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductCellContent'
CREATE INDEX [IX_FK_ProductCellContent]
ON [dbo].[CellContents]
    ([Product_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------