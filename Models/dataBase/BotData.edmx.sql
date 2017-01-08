
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/08/2017 13:51:15
-- Generated from EDMX file: C:\Users\yochai\Source\Repos\Bot-Application2\Bot Application1\dataBase\BotData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(150)  NOT NULL,
    [UserName] nvarchar(150)  NOT NULL,
    [Channel] nvarchar(150)  NOT NULL,
    [created] datetime  NOT NULL,
    [Message] nvarchar(500)  NULL,
    [UserGender] nvarchar(50)  NULL,
    [UserClass] nvarchar(150)  NULL
);
GO

-- Creating table 'QuestionSet'
CREATE TABLE [dbo].[QuestionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Category] nvarchar(max)  NOT NULL,
    [SubCategory] nvarchar(max)  NOT NULL,
    [QuestionText] nvarchar(max)  NOT NULL,
    [AnswerText] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuestionSet'
ALTER TABLE [dbo].[QuestionSet]
ADD CONSTRAINT [PK_QuestionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------