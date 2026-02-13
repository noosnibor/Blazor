CREATE PROCEDURE [dbo].[spSelectUserPermission]
	@pstrUsername		VARCHAR(50),
	@pstrLocationKey	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	 SELECT p.flngPermissionKey

	 FROM tblUser u

	 INNER JOIN tblPermission p
	 ON p.flngRoleKey		= u.flngRoleKey

	 WHERE u.fstrUsername	= @pstrUsername
	 AND u.fstrLocationKey	= @pstrLocationKey
	 AND flngVer			= 0
END

