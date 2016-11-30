-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	Fetches All the Available Skills for the App.
-- Unit Test : EXEC [GET_APPLICATION_SKILLS]
-- =============================================

CREATE PROCEDURE [GET_APPLICATION_SKILLS]
AS
BEGIN

	SELECT SkillId
		  ,Name	 
	FROM [SKILLS]
	ORDER BY Name ASC
	
END