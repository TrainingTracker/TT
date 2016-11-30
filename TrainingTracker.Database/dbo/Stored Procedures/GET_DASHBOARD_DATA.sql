
-- =============================================
-- Author:		<Shekhar Pankaj>
-- Create date: <29 July 2016>
-- Description:	<Fetches Data for Dashboard>
-- =============================================
CREATE PROCEDURE [dbo].[GET_DASHBOARD_DATA]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
 SET NOCOUNT ON;
 
	SELECT 
	   usr.[UserId]
      ,usr.[FirstName] + ' ' + usr.[LastName] AS FullName
	  ,usr.DateAddedToSystem As DateAddedToSystem
	  ,fb.FeedbackType
	  ,fb.FeedbackText
	  ,fb.Title
	  ,CASE WHEN fb.FeedbackType = 2 THEN fb.SkillId 
			ELSE  NULL
	   END AS [SkillId]
	  ,CASE WHEN fb.FeedbackType = 2 THEN s.Name 
			ELSE  NULL
	   END AS [SkillName]
	  ,CASE 
			WHEN fb.FeedbackType = 2  THEN ( SELECT ROUND(AVG(CAST(fbTemp.[Rating] AS FLOAT)),0)	
										   FROM Feedback fbTemp 
										   INNER JOIN [Skills] sTemp ON fbTemp.SkillId = sTemp.SkillId
										   WHERE  fbTemp.SkillId = fb.SkillId
										   GROUP BY fbTemp.SkillId,sTemp.Name )	
			ELSE fb.Rating  
	   END AS [Rating]
	  ,au.[FirstName] + ' ' + au.[LastName] AS AddedBy
	  ,CASE 
			WHEN fb.FeedbackType = 2 THEN NULL
			ELSE fb.AddedOn  
	   END AS [AddedOn]
	  ,CASE 
			WHEN fb.FeedbackType = 5 THEN fb.StartDate
			ELSE NULL 
	   END AS [StartDate]
	   ,CASE 
			WHEN fb.FeedbackType = 5 THEN fb.EndDate
			ELSE NULL 
	   END AS [EndDate]
	  	  
    FROM [dbo].[User] usr
	LEFT JOIN Feedback fb on fb.AddedFor =usr.UserId
	LEFT JOIN [User] au on au.UserId = fb.AddedBy
	LEFT JOIN Skills s on fb.SkillId =s.SkillId	

    WHERE usr.[IsTrainee] = 1 AND usr.[IsActive] = 1
    ORDER BY usr.[FirstName],fb.FeedbackType,fb.SkillId ,fb.AddedOn DESC
	
END