/*2017-10-28*/
/*Thêm tính năng chỉnh sửa template menu*/
/*-------------------------------------------------*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MenuItem]') AND name = 'IsTemplate')
BEGIN
	SET ANSI_PADDING ON

	ALTER TABLE dbo.MenuItem ADD  IsTemplate bit DEFAULT 0

	SET ANSI_PADDING OFF
END


GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MenuItem]') AND name = 'TemplateId')
BEGIN
	SET ANSI_PADDING ON

	ALTER TABLE dbo.MenuItem ADD  TemplateId INT NULL

	SET ANSI_PADDING OFF
END


go

/*-------------------------------------------------*/


/*2017-10-31*/
/*Thêm trang tĩnh AboutUs*/
/*-------------------------------------------------*/

IF NOT EXISTS (SELECT * FROM dbo.Gallery WHERE [Key] = 'page_About' AND IsDeleted = 0)
BEGIN
	INSERT INTO dbo.Gallery
	        ( ApprovalStatus ,
	          IsOnsite ,
	          MsId ,
	          [Key] ,
	          IsPredefined ,
	          CreatedBy ,
	          CreatedDate ,
	          LastUpdatedBy ,
	          LastUpdatedDate ,
	          IsDeleted
	        )
	VALUES  ( 2 , -- ApprovalStatus - int
	          1 , -- IsOnsite - bit
	          (SELECT TOP 1 MsId FROM dbo.Microsite WHERE IsRootSite = 1 AND IsDeleted = 0) , -- MsId - int
	          'page_About' , -- Key - varchar(50)
	          0 , -- IsPredefined - bit
	          0 , -- CreatedBy - int
	          GETDATE() , -- CreatedDate - datetime
	          0 , -- LastUpdatedBy - int
	          GETDATE() , -- LastUpdatedDate - datetime
	          0  -- IsDeleted - bit
	        )
	
	DECLARE @galleryId INT =  SCOPE_IDENTITY()

	INSERT INTO dbo.GalleryContent
	        ( GalleryId ,
	          Name ,
	          ShortDescription ,
	          Description ,
	          RewriteUrl ,
	          LangShortName ,
	          MetaTitle ,
	          MetaDescription ,
	          MetaKeyword ,
	          CreatedBy ,
	          CreatedDate ,
	          LastUpdatedBy ,
	          LastUpdatedDate ,
	          IsDeleted
	        )
	VALUES  ( @galleryId , -- GalleryId - int
	          N'About' , -- Name - nvarchar(500)
	          N'' , -- ShortDescription - nvarchar(512)
	          N'' , -- Description - nvarchar(2048)
	          '' , -- RewriteUrl - varchar(250)
	          'vi' , -- LangShortName - varchar(5)
	          N'' , -- MetaTitle - nvarchar(1024)
	          N'' , -- MetaDescription - nvarchar(1024)
	          N'' , -- MetaKeyword - nvarchar(1024)
	          0 , -- CreatedBy - int
	          GETDATE() , -- CreatedDate - datetime
	          0 , -- LastUpdatedBy - int
	          GETDATE() , -- LastUpdatedDate - datetime
	          0  -- IsDeleted - bit
	        )
END

IF NOT EXISTS (SELECT * FROM dbo.Article WHERE [Key] = 'about' AND IsDeleted = 0)
BEGIN 

	INSERT INTO dbo.Article
			( [Key] ,
			  ArticleCategoryId ,
			  ArticleCategoryOthersId ,
			  IsHot ,
			  IsFocus ,
			  Status ,
			  Type ,
			  IsVipPromotion ,
			  StartDate ,
			  EndDate ,
			  CityId ,
			  MsId ,
			  CreatedBy ,
			  CreatedDate ,
			  LastUpdatedBy ,
			  LastUpdatedDate ,
			  IsDeleted
			)
	VALUES  ( 'about' , -- Key - varchar(50)
			  null , -- ArticleCategoryId - int
			  null , -- ArticleCategoryOthersId - varchar(50)
			  0 , -- IsHot - bit
			  0 , -- IsFocus - bit
			  2 , -- Status - int
			  3 , -- Type - int
			  0 , -- IsVipPromotion - bit
			  GETDATE() , -- StartDate - datetime
			  GETDATE() , -- EndDate - datetime
			  0 , -- CityId - int
			  (SELECT TOP 1 MsId FROM dbo.Microsite WHERE IsRootSite = 1 AND IsDeleted = 0) , -- MsId - int
			  0 , -- CreatedBy - int
			  GETDATE() , -- CreatedDate - datetime
			  0 , -- LastUpdatedBy - int
			  GETDATE() , -- LastUpdatedDate - datetime
			  0  -- IsDeleted - bit
			)

	DECLARE @ArticleId INT =  SCOPE_IDENTITY()

	INSERT INTO dbo.ArticleContent
			( ArticleId ,
			  Title ,
			  Title2 ,
			  TitleWithoutAccent ,
			  Title2WithoutAccent ,
			  Rewrite ,
			  ShortDescription ,
			  ArticleUrl ,
			  Body ,
			  ImageUrl ,
			  TargetLinkImage ,
			  ImageMobileUrl ,
			  TargetLinkImageMobile ,
			  LangShortName ,
			  MetaTitle ,
			  MetaDescription ,
			  MetaKeyword ,
			  ApprovalStatus ,
			  CreatedBy ,
			  CreatedDate ,
			  LastUpdatedBy ,
			  LastUpdatedDate ,
			  IsDeleted
			)
	VALUES  ( @ArticleId , -- ArticleId - int
			  N'About Us' , -- Title - nvarchar(512)
			  N'' , -- Title2 - nvarchar(512)
			  'about us' , -- TitleWithoutAccent - varchar(512)
			  '' , -- Title2WithoutAccent - varchar(512)
			  N'' , -- Rewrite - nvarchar(256)
			  N'' , -- ShortDescription - nvarchar(2048)
			  N'' , -- ArticleUrl - nvarchar(1024)
			  '<p>1</p><div data-gallery-id="page_About" class="cke-gallery" contenteditable="false"><p class="cke-gallery-preview">&nbsp;</p></div><p>2</p>' , -- Body - ntext
			  '' , -- ImageUrl - varchar(512)
			  '' , -- TargetLinkImage - varchar(512)
			  '' , -- ImageMobileUrl - varchar(512)
			  '' , -- TargetLinkImageMobile - varchar(512)
			  'vi' , -- LangShortName - varchar(5)
			  N'' , -- MetaTitle - nvarchar(1024)
			  N'' , -- MetaDescription - nvarchar(1024)
			  N'' , -- MetaKeyword - nvarchar(1024)
			  2 , -- ApprovalStatus - int
			  0 , -- CreatedBy - int
			  GETDATE() , -- CreatedDate - datetime
			  0 , -- LastUpdatedBy - int
			  GETDATE() , -- LastUpdatedDate - datetime
			  0  -- IsDeleted - bit
			)
END

GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[MicrositeType]') AND name = 'Sort')
BEGIN
	SET ANSI_PADDING ON

	ALTER TABLE dbo.MicrositeType ADD  Sort INT DEFAULT 0

	SET ANSI_PADDING OFF
END

GO
