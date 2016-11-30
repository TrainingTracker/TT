
-- =============================================
-- Author:		HARSH WARDHAN
-- Create date: 21st July, 2016
-- Description:	Adds feedback.
-- =============================================
CREATE PROCEDURE [dbo].[ADD_FEEDBACK]
	@FeedbackText VARCHAR(MAX),
	@Title VARCHAR(100),
	@FeedbackType  int,
	@ProjectId  int = null,
	@SkillId int = null,
	@Rating int = null,
	@AddedBy int,
	@AddedFor int,
	@StartDate date = null,
	@EndDate date = null,
	@AddedOn DateTime = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Feedback]
           ([FeedbackText]
           ,[Title]
           ,[FeedbackType]
           ,[ProjectId]
           ,[SkillId]
           ,[Rating]
           ,[AddedBy]
           ,[AddedFor]
		   ,[StartDate]
		   ,[EndDate]
		   ,[AddedOn])
     VALUES
           (@FeedbackText
		   ,@Title
		   ,@FeedbackType
		   ,@ProjectId
		   ,@SkillId
		   ,@Rating
		   ,@AddedBy
		   ,@AddedFor
		   ,@StartDate
		   ,@EndDate
		   ,ISNULL(@AddedOn,GETDATE()))

SELECT @InsertedId= SCOPE_IDENTITY()  

IF(@FeedbackType=2)
BEGIN
	IF NOT EXISTS(SELECT Count(Id) FROM UserSkillMapping WHERE UserId=@AddedFor AND SkillId = @SkillId)
	BEGIN
	INSERT INTO UserSkillMapping
	VALUES
	(
		 @SkillId
		,@AddedFor
		,GETDATE()
		,@AddedBy	
	)
	END
END

SELECT @InsertedId
END