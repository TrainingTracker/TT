-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	Scenario:
--				1 = Get feedbacks on user id.
--				2 = Gets user feedbacks with pagination.
-- EXEC [GET_USER_FEEDBACKS] @UserId=20,@PageSize=15,@FeedbackId=1
-- =============================================
CREATE PROCEDURE [dbo].[GET_USER_FEEDBACKS]
@UserId INT,
@StartDate DATE = null,
@EndDate DATE = null,
@FeedbackId INT = null,
@PageSize INT = 5,
@Offset INT = 0,
@Scenario INT=0,
@StartAddedOn DATE = null,
@EndAddedOn DATE = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@FeedbackId=0)
	BEGIN
		SET @FeedbackId=null
	END

	BEGIN 
		SELECT DISTINCT [FeedbackId]
		  ,[FeedbackText]
		  ,[Title]
		  ,[FeedbackType]
		  ,[Description] AS [FeedbackTypeName]
		  ,[ProjectId]
		  ,[SkillId]
		  ,[Rating]
		  ,[AddedBy]
		  ,u.[FirstName] + ' ' + u.[LastName] AS AddedByUser
		  ,u.ProfilePictureName AS AddedByUserImage
		  ,[AddedFor]
		  ,[AddedOn]
		  ,[StartDate]
		  ,[EndDate]
		  , (SELECT COUNT(ThreadId) From FeedbackThread ft WHERE ft.FeedbackId= f.FeedbackId ) As ThreadCount
		FROM [dbo].[Feedback] f
		JOIN [User] u
		ON f.[AddedBy] = u.[UserId]
		JOIN [FeedbackType] t
		ON f.[FeedbackType] = t.[FeedbackTypeId]
		WHERE f.[AddedFor] = @UserId AND 
			  f.[FeedbackType] = ISNULL(@FeedbackId,f.[FeedbackType]) AND 
			  f.[StartDate] >= ISNULL(@StartDate,f.[StartDate]) AND 
			  f.[EndDate] >= ISNULL(@EndDate,f.[EndDate])  AND 
			  ( CAST( f.[AddedOn] AS DATE) between CAST( ISNULL(@StartAddedOn,f.[AddedOn]) AS DATE) 
			  AND CAST( ISNULL(@EndAddedOn,f.[AddedOn]) AS DATE) )

		ORDER BY [AddedOn] DESC
		OFFSET  @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY 
	END

END