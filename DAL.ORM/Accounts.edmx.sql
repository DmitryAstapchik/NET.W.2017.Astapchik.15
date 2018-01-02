
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/02/2018 01:11:09
-- Generated from EDMX file: E:\studying\programming\EPAM training\GitHub\NET.W.2017.Astapchik.15\DAL.ORM\Accounts.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Accounts];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_OwnerBankAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankAccountSet] DROP CONSTRAINT [FK_OwnerBankAccount];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BankAccountSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankAccountSet];
GO
IF OBJECT_ID(N'[dbo].[OwnerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OwnerSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BankAccountSet'
CREATE TABLE [dbo].[BankAccountSet] (
    [IBAN] nvarchar(50)  NOT NULL,
    [Balance] decimal(18,0)  NOT NULL,
    [BonusPoints] real  NOT NULL,
    [AccountType] nvarchar(max)  NOT NULL,
    [OwnerPID] nvarchar(50)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AccountOwnerSet'
CREATE TABLE [dbo].[AccountOwnerSet] (
    [PassportID] nvarchar(50)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IBAN] in table 'BankAccountSet'
ALTER TABLE [dbo].[BankAccountSet]
ADD CONSTRAINT [PK_BankAccountSet]
    PRIMARY KEY CLUSTERED ([IBAN] ASC);
GO

-- Creating primary key on [PassportID] in table 'AccountOwnerSet'
ALTER TABLE [dbo].[AccountOwnerSet]
ADD CONSTRAINT [PK_AccountOwnerSet]
    PRIMARY KEY CLUSTERED ([PassportID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OwnerPID] in table 'BankAccountSet'
ALTER TABLE [dbo].[BankAccountSet]
ADD CONSTRAINT [FK_OwnerBankAccount]
    FOREIGN KEY ([OwnerPID])
    REFERENCES [dbo].[AccountOwnerSet]
        ([PassportID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OwnerBankAccount'
CREATE INDEX [IX_FK_OwnerBankAccount]
ON [dbo].[BankAccountSet]
    ([OwnerPID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------