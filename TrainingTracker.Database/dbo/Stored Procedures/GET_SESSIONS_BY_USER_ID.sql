-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GET_SESSIONS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT s.[SessionId]
		,[Title]
	FROM [dbo].[Session] s
	JOIN [UserSessionMapping] m
	ON s.[SessionId] = m.[SessionId]
	WHERE m.UserId = @UserId
				
END