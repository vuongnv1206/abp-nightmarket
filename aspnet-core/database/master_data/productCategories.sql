USE [NightMarket]
GO

INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescription]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Phone'
           ,'C1'
           ,'phone'
           ,1
           ,'picture'
           ,1
           ,1
           ,null
           ,N'Phone category'
           ,null
           ,null
           ,getdate()
           ,null)

		   INSERT INTO [dbo].[AppProductCategories]
           ([Id]
           ,[Name]
           ,[Code]
           ,[Slug]
           ,[SortOrder]
           ,[CoverPicture]
           ,[Visibility]
           ,[IsActive]
           ,[ParentId]
           ,[SeoMetaDescription]
           ,[ExtraProperties]
           ,[ConcurrencyStamp]
           ,[CreationTime]
           ,[CreatorId])
     VALUES
           (newid()
           ,N'Laptop'
           ,'C2'
           ,'laptop'
           ,1
           ,'picture'
           ,1
           ,1
           ,null
           ,N'Laptop category'
           ,null
           ,null
           ,getdate()
           ,null)

