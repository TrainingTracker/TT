-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- Test  - EXEC GET_SKILLS_BY_USER_ID 20
-- =============================================
CREATE PROCEDURE [dbo].[GET_SKILLS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--SELECT s.[SkillId]
	--	,[Name]
	--	,(SELECT ROUND(AVG(CAST([Rating] AS FLOAT)),0)
	--		FROM [dbo].[Feedback] f
	--		//JOIN [FeedbackSkillMapping] sk
	--		ON sk.[FeedbackId] = f.[FeedbackId] 
	--		AND sk.[UserId] = @UserId 
	--		AND sk.[SkillId] = s.[SkillId]) AS Rating
	--FROM [dbo].[Skills] s
	--JOIN [UserSkillMapping] m
	--ON s.SkillId = m.SkillId
	--WHERE m.UserId = @UserId

	SELECT  f.SkillId
		  , s.Name
		  , ROUND(AVG(CAST(f.[Rating] AS FLOAT)),0) AS [Rating]
	FROM [Feedback] f
	INNER JOIN [Skills] s ON f.SkillId = s.SkillId
	WHERE f.AddedFor = @UserId
	GROUP BY f.SkillId,s.Name 
				
END