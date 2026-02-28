CREATE PROCEDURE [dbo].[spUpdateUserPassword]
	@pstrUsername NVARCHAR(50),
    @pstrPassword NVARCHAR(255),
    @pblnPasswordChanged BIT
AS
BEGIN	
	UPDATE tblUser SET
	fstrPassword		= @pstrPassword,
	fblnPasswordChanged = 1
	WHERE fstrUsername	= @pstrUsername
	AND flngVer			= 0
END

