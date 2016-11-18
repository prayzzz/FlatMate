--
-- Script
--

CREATE SCHEMA [List]
GO

-- Itemlist
CREATE TABLE [List].[ItemList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[Description] [varchar](1000) NULL,
	[IsPublic] [bit] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_ItemList] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [List].[ItemList]  WITH CHECK ADD  CONSTRAINT [FK_ItemList_User] FOREIGN KEY([UserId]) REFERENCES [Account].[User] ([Id])
GO

ALTER TABLE [List].[ItemList] CHECK CONSTRAINT [FK_ItemList_User]
GO

-- ItemListGroup
CREATE TABLE [List].[ItemListGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ItemListId] [int] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_ItemListGroup] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [List].[ItemListGroup]  WITH CHECK ADD  CONSTRAINT [FK_ItemListGroup_ItemList] FOREIGN KEY([ItemListId]) REFERENCES [List].[ItemList] ([Id])
GO

ALTER TABLE [List].[ItemListGroup] CHECK CONSTRAINT [FK_ItemListGroup_ItemList]
GO

ALTER TABLE [List].[ItemListGroup]  WITH CHECK ADD  CONSTRAINT [FK_ItemListGroup_User] FOREIGN KEY([UserId]) REFERENCES [Account].[User] ([Id])
GO

ALTER TABLE [List].[ItemListGroup] CHECK CONSTRAINT [FK_ItemListGroup_User]
GO

-- Item
CREATE TABLE [List].[Item] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ItemListId] [int] NOT NULL,
	[ItemListGroupId] [int] NULL,
	[LastModified] [datetime] NOT NULL,
    [Order] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Value] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [List].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemList] FOREIGN KEY([ItemListId]) REFERENCES [List].[ItemList] ([Id])
GO

ALTER TABLE [List].[Item] CHECK CONSTRAINT [FK_Item_ItemList]
GO

ALTER TABLE [List].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemListGroup] FOREIGN KEY([ItemListGroupId]) REFERENCES [List].[ItemListGroup] ([Id])
GO

ALTER TABLE [List].[Item] CHECK CONSTRAINT [FK_Item_ItemListGroup]
GO

ALTER TABLE [List].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_User] FOREIGN KEY([UserId]) REFERENCES [Account].[User] ([Id])
GO

ALTER TABLE [List].[Item] CHECK CONSTRAINT [FK_Item_User]
GO

--
-- Update Info
--


INSERT INTO [Infrastructure].[DbScripts] ([FileName]) 
VALUES ('2016-11-17_15-43-05_create_list_tables');