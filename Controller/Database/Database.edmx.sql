
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2017 15:16:28
-- Generated from EDMX file: C:\Users\Alex\Documents\Visual Studio 2017\Projects\Diploma\Controller\Database\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Diploma];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsersReports]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReportsSet] DROP CONSTRAINT [FK_UsersReports];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersProfiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfilesSet] DROP CONSTRAINT [FK_UsersProfiles];
GO
IF OBJECT_ID(N'[dbo].[FK_ReportsPoints]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PointsSet] DROP CONSTRAINT [FK_ReportsPoints];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UsersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersSet];
GO
IF OBJECT_ID(N'[dbo].[ReportsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReportsSet];
GO
IF OBJECT_ID(N'[dbo].[PointsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PointsSet];
GO
IF OBJECT_ID(N'[dbo].[UrlsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UrlsSet];
GO
IF OBJECT_ID(N'[dbo].[ProfilesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfilesSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(128)  NOT NULL,
    [Password] nvarchar(128)  NOT NULL,
    [AttemptsLeft] tinyint  NOT NULL,
    [Online] bit  NOT NULL
);
GO

-- Creating table 'ReportsSet'
CREATE TABLE [dbo].[ReportsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Time] datetime  NOT NULL,
    [VirtualUsers] int  NOT NULL,
    [Timeout] int  NOT NULL,
    [RequestDuration] int  NOT NULL,
    [Duration] int  NOT NULL,
    [Strategy] tinyint  NOT NULL,
    [UserId] int  NOT NULL,
    [UrlGroup] int  NOT NULL
);
GO

-- Creating table 'PointsSet'
CREATE TABLE [dbo].[PointsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [X] int  NOT NULL,
    [Y] int  NOT NULL,
    [ReportId] int  NOT NULL
);
GO

-- Creating table 'UrlsSet'
CREATE TABLE [dbo].[UrlsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [Group] int  NOT NULL
);
GO

-- Creating table 'ProfilesSet'
CREATE TABLE [dbo].[ProfilesSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [UrlGroup] int  NOT NULL,
    [VirtualUsers] int  NOT NULL,
    [Timeout] int  NOT NULL,
    [RequestDuration] int  NOT NULL,
    [Duration] int  NOT NULL,
    [Strategy] tinyint  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReportsSet'
ALTER TABLE [dbo].[ReportsSet]
ADD CONSTRAINT [PK_ReportsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PointsSet'
ALTER TABLE [dbo].[PointsSet]
ADD CONSTRAINT [PK_PointsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UrlsSet'
ALTER TABLE [dbo].[UrlsSet]
ADD CONSTRAINT [PK_UrlsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProfilesSet'
ALTER TABLE [dbo].[ProfilesSet]
ADD CONSTRAINT [PK_ProfilesSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'ReportsSet'
ALTER TABLE [dbo].[ReportsSet]
ADD CONSTRAINT [FK_UsersReports]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersReports'
CREATE INDEX [IX_FK_UsersReports]
ON [dbo].[ReportsSet]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'ProfilesSet'
ALTER TABLE [dbo].[ProfilesSet]
ADD CONSTRAINT [FK_UsersProfiles]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersProfiles'
CREATE INDEX [IX_FK_UsersProfiles]
ON [dbo].[ProfilesSet]
    ([UserId]);
GO

-- Creating foreign key on [ReportId] in table 'PointsSet'
ALTER TABLE [dbo].[PointsSet]
ADD CONSTRAINT [FK_ReportsPoints]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[ReportsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportsPoints'
CREATE INDEX [IX_FK_ReportsPoints]
ON [dbo].[PointsSet]
    ([ReportId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------