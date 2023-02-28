BEGIN TRANSACTION test1

DECLARE @tmp TABLE (
	MsId INT, 
	Title NVARCHAR(100), 
	ShortName NVARCHAR(5), 
	Rewrite NVARCHAR(500),
	ShortDescription NVARCHAR(500),
    Description NVARCHAR(500),
    ImageUrl NVARCHAR(500),
    TargetLinkImage NVARCHAR(500),
    ImageMobileUrl NVARCHAR(500),
    TargetLinkImageMobile NVARCHAR(500),
    BannerUrl NVARCHAR(500),
    MetaTitle NVARCHAR(500),
    MetaDescription NVARCHAR(500),
    MetaKeyword NVARCHAR(500),
    ApprovalStatus INT
)

INSERT INTO @tmp
        ( MsId, 
		  Title, 
		  ShortName, 
		  Rewrite,
		  ShortDescription ,
          Description ,
          ImageUrl ,
          TargetLinkImage ,
          ImageMobileUrl ,
          TargetLinkImageMobile ,
          BannerUrl ,
          MetaTitle ,
          MetaDescription ,
          MetaKeyword ,
          ApprovalStatus )
SELECT DISTINCT M.MsId, MC.Title, L.ShortName, MC.Rewrite,
		MC.ShortDescription ,
          MC.Description ,
          MC.ImageUrl ,
          MC.TargetLinkImage ,
          MC.ImageMobileUrl ,
          MC.TargetLinkImageMobile ,
          MC.BannerUrl ,
          MC.MetaTitle ,
          MC.MetaDescription ,
          MC.MetaKeyword ,
          MC.ApprovalStatus 
FROM dbo.[Language] L, dbo.Microsite M LEFT JOIN dbo.MicrositeContent MC ON MC.MsId = M.MsId
WHERE M.IsDeleted <> 1 AND L.IsOnsite =1 

--SELECT DISTINCT * FROM @tmp WHERE MsId = 1

MERGE INTO MicrositeContent AS TARGET
USING @tmp AS SOURCE
ON (TARGET.MsId = SOURCE.MsId AND TARGET.LangShortName =  SOURCE.ShortName)
WHEN NOT MATCHED THEN
	INSERT (
			MsId ,
          Title ,
          Rewrite ,
          ShortDescription ,
          Description ,
          ImageUrl ,
          TargetLinkImage ,
          ImageMobileUrl ,
          TargetLinkImageMobile ,
          BannerUrl ,
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
	VALUES
		(
			SOURCE.MsId,--MsId ,
			SOURCE.Title,--Title ,
			SOURCE.Rewrite,--Rewrite ,
			SOURCE.ShortDescription,--ShortDescription ,
			SOURCE.Description,--Description ,
			SOURCE.ImageUrl,--ImageUrl ,
			SOURCE.TargetLinkImage,--TargetLinkImage ,
			SOURCE.ImageMobileUrl,--ImageMobileUrl ,
			SOURCE.TargetLinkImageMobile,--TargetLinkImageMobile ,
			SOURCE.BannerUrl,--BannerUrl ,
			SOURCE.ShortName,--LangShortName ,
			SOURCE.MetaTitle,--MetaTitle ,
			SOURCE.MetaDescription,--MetaDescription ,
			SOURCE.MetaKeyword,--MetaKeyword ,
			SOURCE.ApprovalStatus,--ApprovalStatus ,
			0,--CreatedBy ,
			GETDATE(),--CreatedDate ,
			0,--LastUpdatedBy ,
			Getdate(),--LastUpdatedDate ,
			0--IsDeleted
		);

SELECT MsId, LangShortName, Title, Rewrite FROM dbo.MicrositeContent ORDER BY MsId, LangShortName

ROLLBACK TRANSACTION test1

