
CREATE PROCEDURE [dbo].[ADD_UPDATE_SESSION_DETAILS]
(
 @Presenter INT,
 @Title NVARCHAR(MAX)='',
 @Description NVARCHAR(MAX)='',
 @Date DATETIME,
 @SessionId INT,
 @AttendeeCsv NVARCHAR(MAX)=''

)
AS 
BEGIN

IF(@SessionId=0)
BEGIN
	INSERT INTO [Session](Title,[Description],Presenter,AddedOn,SessionDate)
	VALUES(@Title,@Description,@Presenter,GETDATE(),@Date)

	SELECT @SessionId= IDENT_CURRENT('Session');
END
ELSE
	BEGIN
	 UPDATE [Session] 
	 SET Title=@Title,[Description]=@Description,SessionDate=@Date
	 WHERE SessionId=@SessionId

	 DELETE FROM UserSessionMapping
	 WHERE SessionId=@SessionId

	 
	END

IF EXISTS(SELECT Id From [dbo].[ufn_CSVToTable](@AttendeeCsv,','))
	BEGIN
	INSERT INTO UserSessionMapping(UserId,SessionId,AddedBy)
	SELECT Value,@SessionId,@Presenter From [dbo].[ufn_CSVToTable](@AttendeeCsv,',')
	END

SELECT @SessionId
END