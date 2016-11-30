CREATE FUNCTION [dbo].[ufn_CSVToTable] ( @StringInput NVARCHAR(4000), @Delimiter nvarchar(1))
RETURNS @OutputTable TABLE ( [Id] INT Identity(1,1), [Value] NVARCHAR(10) )
AS
BEGIN

    DECLARE @String    VARCHAR(10)

    WHILE LEN(@StringInput) > 0
    BEGIN
        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [Value] )
        VALUES ( @String )
    END

    RETURN
END