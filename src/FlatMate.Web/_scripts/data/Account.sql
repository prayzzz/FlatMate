USE [flatmate]
GO
SET IDENTITY_INSERT [Account].[User] ON 

INSERT [Account].[User] ([Id], [CreationDate], [Email], [LastModified], [LastName], [Name], [PasswordHash], [Salt], [Username]) VALUES (1, CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user1@user1.de', CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user1', N'user1', N'/nbLjcpYFZ3FDEh9tj/HAIS7u3Cwym3J7N7TOuSDzGM=', N'BgAsvHtOBKeM/nvAvLCbWA==', N'user1')
INSERT [Account].[User] ([Id], [CreationDate], [Email], [LastModified], [LastName], [Name], [PasswordHash], [Salt], [Username]) VALUES (2, CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user2@user2.de', CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user2', N'user2', N'5ykWS0o1iukIXmwXOoIrfxasXnaHhIMF6sqZ4RUygfw=', N'Azk9NnWnQ+IkKLBb4kVO5A==', N'user2')
INSERT [Account].[User] ([Id], [CreationDate], [Email], [LastModified], [LastName], [Name], [PasswordHash], [Salt], [Username]) VALUES (3, CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user3@user3.de', CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user3', N'user3', N'0/MPnio2Am5pOG+Pw2ba+DpRpCCKTduH4ymDIig+cZU=', N'xxjbuKKEhj4aTgbBwocnYQ==', N'user3')
INSERT [Account].[User] ([Id], [CreationDate], [Email], [LastModified], [LastName], [Name], [PasswordHash], [Salt], [Username]) VALUES (4, CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user4@user4.de', CAST(N'2016-11-18 19:36:17.000' AS DateTime), N'user4', N'user4', N'bhcP449c/AbsKl2N3Zsvnx+AAbtW1xJ4nWO0FoBUXTg=', N'p1ZjoxuK3qF4NBDiK0zJ0w==', N'user4')
SET IDENTITY_INSERT [Account].[User] OFF
