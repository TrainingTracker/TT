CREATE PROCEDURE [dbo].[GET_ALL_USERS]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [UserId]
      ,[FirstName]
      ,[LastName]
      ,[UserName]
      ,[Password]
      ,[Email]
      ,[Designation]
      ,[ProfilePictureName]
      ,[IsFemale]
      ,[IsAdministrator]
      ,[IsTrainer]
      ,[IsTrainee]
      ,[IsManager]
      ,[IsActive]
  FROM [dbo].[User]
  ORDER BY [FirstName]
  
END