CREATE PROCEDURE [dbo].[spDeletePermission]
	@plngPermissionKey	INT,
	@plngRoleKey		INT
AS
BEGIN
	DELETE FROM tblPermission WHERE flngPermissionKey = @plngPermissionKey AND flngRoleKey = @plngRoleKey
END
