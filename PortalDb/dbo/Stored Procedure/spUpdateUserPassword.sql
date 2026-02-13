CREATE PROCEDURE [dbo].[spUpdateUserPassword]
	@plngUserKey	INT,
	@pstrPassword	NVARCHAR(255)
AS
BEGIN	
	UPDATE tblUser SET
	fstrPassword		= @pstrPassword,
	fblnPasswordChanged = 1
	WHERE flngUserKey	= @plngUserKey
	AND flngVer			= 0
END
