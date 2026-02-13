CREATE PROCEDURE [dbo].[spSelectPermission]
	@plngPermissionKey  INT,
	@plngRoleKey        INT
AS
BEGIN
	SET NOCOUNT ON;

    SELECT flngPermissionKey,
           flngRoleKey
    FROM    tblPermission
    WHERE (@plngPermissionKey   IS NULL OR flngPermissionKey    = @plngPermissionKey)
    AND (@plngRoleKey           IS NULL OR flngRoleKey          = @plngRoleKey)
END