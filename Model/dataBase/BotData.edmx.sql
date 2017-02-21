
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/19/2017 09:18:48
-- Generated from EDMX file: C:\Users\yochai\Source\Repos\Bot-Application2\Model\dataBase\BotData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [mindcet];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SubQuestion_Question]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubQuestion] DROP CONSTRAINT [FK_SubQuestion_Question];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[botphrase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[botphrase];
GO
IF OBJECT_ID(N'[dbo].[media]', 'U') IS NOT NULL
    DROP TABLE [dbo].[media];
GO
IF OBJECT_ID(N'[dbo].[Question]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Question];
GO
IF OBJECT_ID(N'[dbo].[SubQuestion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubQuestion];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'botphrase'
CREATE TABLE [dbo].[botphrase] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Pkey] nvarchar(200)  NULL,
    [Text] nvarchar(max)  NULL,
    [Flags] nvarchar(max)  NULL
);
GO

-- Creating table 'media'
CREATE TABLE [dbo].[media] (
    [id] int IDENTITY(1,1) NOT NULL,
    [type] nvarchar(50)  NULL,
    [mediaKey] nvarchar(100)  NULL,
    [value] nvarchar(200)  NULL,
    [flags] nvarchar(200)  NULL
);
GO

-- Creating table 'Question'
CREATE TABLE [dbo].[Question] (
    [QuestionID] nchar(20)  NOT NULL,
    [Category] nvarchar(100)  NULL,
    [SubCategory] nvarchar(100)  NULL,
    [QuestionText] nvarchar(max)  NULL,
    [Flags] nvarchar(max)  NULL
);
GO

-- Creating table 'SubQuestion'
CREATE TABLE [dbo].[SubQuestion] (
    [questionID] nchar(20)  NOT NULL,
    [subQuestionID] nchar(10)  NOT NULL,
    [questionText] nchar(1000)  NULL,
    [answerText] nchar(1000)  NULL,
    [flags] nchar(10)  NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [UserID] nchar(30)  NOT NULL,
    [UserName] nchar(100)  NULL,
    [UserGender] nchar(10)  NULL,
    [UserClass] nchar(10)  NULL,
    [UserCreated] datetime  NULL,
    [UserLastSession] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'botphrase'
ALTER TABLE [dbo].[botphrase]
ADD CONSTRAINT [PK_botphrase]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'media'
ALTER TABLE [dbo].[media]
ADD CONSTRAINT [PK_media]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [QuestionID] in table 'Question'
ALTER TABLE [dbo].[Question]
ADD CONSTRAINT [PK_Question]
    PRIMARY KEY CLUSTERED ([QuestionID] ASC);
GO

-- Creating primary key on [questionID], [subQuestionID] in table 'SubQuestion'
ALTER TABLE [dbo].[SubQuestion]
ADD CONSTRAINT [PK_SubQuestion]
    PRIMARY KEY CLUSTERED ([questionID], [subQuestionID] ASC);
GO

-- Creating primary key on [UserID] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [questionID] in table 'SubQuestion'
ALTER TABLE [dbo].[SubQuestion]
ADD CONSTRAINT [FK_SubQuestion_Question]
    FOREIGN KEY ([questionID])
    REFERENCES [dbo].[Question]
        ([QuestionID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------