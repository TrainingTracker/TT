-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GET_PROJECTS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT p.[ProjectId]
		,[Title]
		,m.[Id] AS UserProjectId
	FROM [dbo].[Project] p
	JOIN [UserProjectMapping] m 
	ON p.ProjectId = m.ProjectId
	WHERE m.UserId = @UserId
				
END