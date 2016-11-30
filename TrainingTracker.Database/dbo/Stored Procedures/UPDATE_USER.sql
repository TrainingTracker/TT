-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_USER]
	@UserId INT,
	@FirstName varchar(100),
	@LastName varchar(100),
	@Username  VARCHAR(200),
	@Password  VARCHAR(200),
	@Email varchar(100),
	@Designation varchar(100),
	@ProfilePictureName varchar(100),
	@IsFemale bit,
	@IsAdministrator bit,
	@IsTrainer bit,
	@IsTrainee bit,
	@IsManager bit,
	@IsActive bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	IF (ISNULL(@Password,'') = '') and (ISNULL(@UserId,0)!=0)
	BEGIN
	UPDATE [dbo].[User]
	SET		[FirstName] = @FirstName 
			,[LastName] = @LastName
           ,[UserName] =@Username
           ,[Email]=@Email
           ,[Designation]=@Designation
           ,[ProfilePictureName]=@ProfilePictureName
           ,[IsFemale]=@IsFemale
           ,[IsAdministrator]=@IsAdministrator
           ,[IsTrainer]=@IsTrainer
           ,[IsTrainee]=@IsTrainee
           ,[IsManager]=@IsManager
           ,[IsActive]=@IsActive
     WHERE UserId=@UserId;
	 END
	 ELSE IF (ISNULL(@UserId,0)!=0)
	 BEGIN
	 UPDATE [dbo].[User]
		SET		[FirstName] = @FirstName 
			,[LastName] = @LastName
           ,[UserName] =@Username
           ,[Password]=@Password
           ,[Email]=@Email
           ,[Designation]=@Designation
           ,[ProfilePictureName]=@ProfilePictureName
           ,[IsFemale]=@IsFemale
           ,[IsAdministrator]=@IsAdministrator
           ,[IsTrainer]=@IsTrainer
           ,[IsTrainee]=@IsTrainee
           ,[IsManager]=@IsManager
           ,[IsActive]=@IsActive
		WHERE UserId=@UserId;
	 END

END