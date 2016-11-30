
CREATE PROCEDURE [dbo].[GET_SESSIONS_ON_FILTERS] --100,'',0,0,26
(
	@PageSize INT = 5,
	@KeyWord NVARCHAR='',
	@SessionType INT=0, 
	@Offset INT = 0,
	@TeamId Int = 0
)

AS
BEGIN
DECLARE @Today DATE = GETDATE()

	SELECT 
		  s.SessionId
		 ,s.Title
		 ,s.[Description]
		 ,s.SessionDate
		 ,usr.FirstName + ' ' + usr.LastName AS PresenterFullName
		 ,s.presenter
		 ,s.VideoFileName
		 ,s.SlideName
		 ,ausr.UserId
		 ,ausr.FirstName
		 ,ausr.LastName
FROM
	(SELECT * 
	 FROM [Session]s
	  ORDER BY SessionDate DESC
		OFFSET  @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY ) AS s
	LEFT JOIN [USER] usr on s.Presenter=usr.UserId
	LEFT JOIN  [UserSessionMapping] usm on usm.SessionId=s.SessionId
	LEFT JOIN [User] ausr on ausr.UserId = usm.UserId
	  WHERE  (
			 (
				   (@SessionType = 1  AND SessionDate >=  @Today)
				OR (@SessionType = 2 AND SessionDate <  @Today)
				OR (@SessionType =0 AND SessionDate=SessionDate)
			 ))

			 AND 
			 (
			    usr.TeamId = CASE @TeamId WHEN 0 THEN usr.TeamId ELSE @TeamId END
			 )
		 
END