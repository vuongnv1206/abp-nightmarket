USE [NightMarket]
GO

INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Apple'
           ,'M1'
           ,'apple'
           ,'picture'
           ,1
           ,1
           ,'US'
           ,null
           ,null
           ,getdate()
           ,null)


		   INSERT INTO [dbo].[AppManufacturers]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[Country]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Miscrosoft'
           ,'M2'
           ,'miscrosoft'
           ,'picture'
           ,1
           ,1
           ,'US'
           ,null
           ,null
           ,getdate()
           ,null)


