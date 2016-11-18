--
-- Script
--

CREATE SCHEMA [Account]
GO

CREATE TABLE [Account].[User] (
	[Id] int NOT NULL IDENTITY (1, 1),    
	[CreationDate] [datetime] NOT NULL,	
	[Email] varchar(100) NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[LastName] varchar(50) NULL,	
    [Name] varchar(50) NOT NULL,
	[PasswordHash] varchar(255) NOT NULL,
	[Salt] varchar(255) NOT NULL,
    [Username] varchar(50) NOT NULL,
) ON [PRIMARY]
GO

ALTER TABLE [Account].[User] ADD CONSTRAINT PK_User PRIMARY KEY CLUSTERED (Id)
WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--
-- Update Info
--


INSERT INTO [Infrastructure].[DbScripts] ([FileName]) 
VALUES ('2016-11-17_15-25-55_create_user_table');