CREATE PROCEDURE [dbo].[spAddPermission]
	@permission dbo.tempPermission READONLY
AS
BEGIN
    INSERT INTO tblPermission
        (
            flngPermissionKey,
            flngRoleKey
        )

        SELECT  flngPermissionKey, 
                flngRoleKey
        FROM @Permission;
END
