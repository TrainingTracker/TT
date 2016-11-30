CREATE PROCEDURE [dbo].[ADD_USER]
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
	SET NOCOUNT ON;

	INSERT INTO [dbo].[User]
           ([FirstName]
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
           ,[IsActive])
OUTPUT Inserted.UserId
     VALUES
           (@FirstName
           ,@LastName
           ,@Username
           ,@Password
           ,@Email
           ,@Designation
           ,@ProfilePictureName
           ,@IsFemale
           ,@IsAdministrator
           ,@IsTrainer
           ,@IsTrainee
           ,@IsManager
		   ,@IsActive)
END