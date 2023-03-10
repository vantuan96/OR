
/****** Object:  UserDefinedFunction [dbo].[SplitListToInt]    Script Date: 10/19/2017 7:02:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[SplitListToInt]
(	
	@list VARCHAR(MAX), @separator VARCHAR(MAX) = ','
)
RETURNS @table TABLE (Value INT)
AS BEGIN
	DECLARE @position INT, @previous INT
	SET @list = @list + @separator
	SET @previous = 1
	SET @position = CHARINDEX(@separator, @list)
	WHILE @position > 0 BEGIN
	IF @position - @previous > 0
		INSERT INTO @table VALUES (CAST(SUBSTRING(@list, @previous, @position - @previous) AS INT))
	IF @position >= LEN(@list) BREAK
	SET @previous = @position + 1
	SET @position = CHARINDEX(@separator, @list, @previous)
END
RETURN
END

