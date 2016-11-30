-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[VALIDATE_USER]
	@Username  VARCHAR(50),
	@Password  VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RecordCount int

	BEGIN
		SELECT @RecordCount = COUNT(*) 
							FROM [User] 
							WHERE [UserName] = @Username 
							AND [Password] = @Password
	END

	BEGIN
		IF @RecordCount > 0 
			SELECT 1 AS IsValid
		ELSE
			SELECT 0 AS IsValid
	END 

END